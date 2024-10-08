﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using FlowZone.shared.Dtos;

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
                ImagePath = GetImagePath(a.ImagePath) 
            }).ToListAsync();

            return Ok(avatars);
        }

        private static string GetImagePath(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return null;
            }

            string imageName = Path.GetFileName(imagePath);
            return $"{imageName}";
        }

        [HttpGet("image/{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);

            if (System.IO.File.Exists(imagePath))
            {
                var imageFileStream = System.IO.File.OpenRead(imagePath);
                return File(imageFileStream, "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]AvatarRequestDto model)
        {
            if (model == null || model.avatarImage == null)
                return BadRequest("Invalid form data");

            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            Directory.CreateDirectory(uploadDir);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.avatarImage.FileName;
            string filePath = Path.Combine(uploadDir, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.avatarImage.CopyToAsync(stream);
            }

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

    }
}
