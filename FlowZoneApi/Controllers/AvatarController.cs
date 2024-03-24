using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using FlowZone.shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Hosting;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly DataContext _context;

        public AvatarController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvatarDto>>> GetAvatars()
        {
            var avatars = await _context.Avatars.Select(a => new AvatarDto
            {
                AvatarId = a.AvatarId,
                avatarName = a.Name,
                avatarPrice = a.Price,
                ImagePath = GetImagePath(a.ImagePath) // Retrieve image path
            }).ToListAsync();

            return Ok(avatars);
        }

        // Helper method to get full image path
        private static string GetImagePath(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return null;
            }
            return Path.Combine("/api/Avatar/image/", imageName);
        }


        [HttpPost]
        public async Task<ActionResult<Avatar>> PostAvatar(AvatarDto avatarDto)
        {
            string FileName = UploadFile(avatarDto);
            var Avatar = new Avatar
            {
                Name = avatarDto.avatarName,
                Price = avatarDto.avatarPrice,
                ImagePath = FileName
            };
            _context.Avatars.Add(Avatar);
            await _context.SaveChangesAsync();

            var savedAvatarDto = new Avatar
            {
                AvatarId = Avatar.AvatarId,
                Name = Avatar.Name,
                Price = Avatar.Price,
                
            };

            // Return the saved AvatarDto along with a 201 Created status
            return CreatedAtAction(nameof(GetAvatars), new { id = savedAvatarDto.AvatarId }, savedAvatarDto);
        }


        private string UploadFile(AvatarDto avatarDto)
        {
            string fileName = null;
            if (avatarDto.avatarImage != null)
            {
                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                Directory.CreateDirectory(uploadDir); // Create directory if it doesn't exist
                fileName = Guid.NewGuid().ToString() + "-" + avatarDto.avatarImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    avatarDto.avatarImage.CopyTo(fileStream);
                }
            }
            return fileName;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvatar(Guid id, AvatarDto avatarDto)
        {
            var avatar = await _context.Avatars.FindAsync(id);

            if (avatar == null)
            {
                return NotFound();
            }

            // Update other properties
            avatar.Name = avatarDto.avatarName;
            avatar.Price = avatarDto.avatarPrice;

            if (avatarDto.avatarImage != null)
            {
                // If a new image is provided, update the image
                string newFileName = UploadFile(avatarDto);
                DeleteFile(avatar.ImagePath); // Delete the old image
                avatar.ImagePath = newFileName;
            }

            _context.Entry(avatar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvatarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvatar(Guid id)
        {
            var avatar = await _context.Avatars.FindAsync(id);
            if (avatar == null)
            {
                return NotFound();
            }

            // Delete associated image file
            DeleteFile(avatar.ImagePath);

            _context.Avatars.Remove(avatar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AvatarExists(Guid id)
        {
            return _context.Avatars.Any(e => e.AvatarId == id);
        }

        private void DeleteFile(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

    }
}
