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


        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetAvatarById(Guid id)
        {
            var avatar = await _context.Avatars.FindAsync(id);

            if (avatar == null)
            {
                return NotFound(); 
            }

            return Ok(avatar);
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
        public async Task<IActionResult> Create([FromForm]AvatarRequestDto model)
        {
            if (model == null || model.avatarImage == null)
                return BadRequest("Invalid form data");

            // Save the file to a root folder
            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            Directory.CreateDirectory(uploadDir); // Create directory if it doesn't exist
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.avatarImage.FileName;
            string filePath = Path.Combine(uploadDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.avatarImage.CopyToAsync(stream);
            }

            // Store the file path in the database
            var Avatar = new Avatar
            {
                Name = model.avatarName,
                Price = model.avatarPrice,
                ImagePath = filePath
            };

            _context.Avatars.Add(Avatar);
            await _context.SaveChangesAsync();

            return Ok("Avatar created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvatar(Guid id, [FromBody] AvatarUpdateRequestDto avatarUpdateRequestDto)
        {
            var avatar = await _context.Avatars.FindAsync(id);

            if (avatar == null)
            {
                return NotFound();
            }

            // Update other properties
            avatar.Name = avatarUpdateRequestDto.avatarName;
            avatar.Price = avatarUpdateRequestDto.avatarPrice;

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
