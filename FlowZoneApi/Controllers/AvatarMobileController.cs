using FlowZone.shared;
using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarMobileController : ControllerBase
    {
        private readonly DataContext _context;

        public AvatarMobileController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ResultWithDataDto<List<AvatarDto>>> GetAvatars()
        {
            var avatars = await _context.Avatars.Select(a => new AvatarDto
            {
                AvatarId = a.AvatarId,
                avatarName = a.Name,
                avatarPrice = a.Price,
                ImagePath = GetImagePath(a.ImagePath) 
            }).ToListAsync();

            var result = ResultWithDataDto<List<AvatarDto>>.Success(avatars);

            return result;
        }

        private static string GetImagePath(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return null;
            }

            Debug.WriteLine($"Original imagePath: {imagePath}");

            string imageName = Path.GetFileName(imagePath);

            Debug.WriteLine($"Extracted imageName: {imageName}");

            string imageUrl = $"{AppConstants.BaseApiUrl}/api/AvatarMobile/image/{imageName}";

            Debug.WriteLine($"Final imageUrl: {imageUrl}");

            return imageUrl;
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

        //[HttpGet("image/{imageName}")]
        //public IActionResult GetImage(string imageName)
        //{
        //   
        //    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);

        //    if (System.IO.File.Exists(imagePath))
        //    {
        //        var contentType = GetContentType(imagePath);

        //        // Read the image file and return as a FileResult
        //        var imageFileStream = System.IO.File.OpenRead(imagePath);
        //        return File(imageFileStream, contentType); 
        //    }
        //    else
        //    {
        //       
        //        return NotFound();
        //    }
        //}
        //private string GetContentType(string filePath)
        //{
        //    var extension = Path.GetExtension(filePath);
        //    switch (extension.ToLower())
        //    {
        //        case ".jpg":
        //        case ".jpeg":
        //            return "image/jpeg";
        //        case ".png":
        //            return "image/png";
        //        default:
        //            return "application/octet-stream";
        //    }
        //}

    }
}
