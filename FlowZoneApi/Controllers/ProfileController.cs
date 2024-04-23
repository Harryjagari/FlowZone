using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using FlowZone.shared.Dtos;
using FlowZone.shared;
using System.Diagnostics;

namespace FlowZoneApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly DataContext _context;

        public ProfileController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ResultWithDataDto<List<ProfileDto>>> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return new ResultWithDataDto<List<ProfileDto>>(false, null, "Unauthorized");
            }

            var user = await _context.Users.FindAsync(Guid.Parse(userId));

            if (user == null)
            {
                return new ResultWithDataDto<List<ProfileDto>>(false, null, "Avatar not found");
            }

            var profileDto = new ProfileDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                EarnedPoints = user.EarnedPoints,
                Address = user.Address,
                ProfilePictureUrl = GetImagePath(user.ProfilePictureUrl)
            };

            var profileDtoList = new List<ProfileDto> { profileDto };

            return new ResultWithDataDto<List<ProfileDto>>(true, profileDtoList, null);
        }


        [HttpPost("set-profile-picture")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]

        public async Task<ResultWithDataDto<string>> SetProfilePicture(Guid avatarId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return new ResultWithDataDto<string>(false, null, "Unauthorized");
            }

            var userAvatar = await _context.UserAvatars
                .Include(ua => ua.Avatar)
                .FirstOrDefaultAsync(ua => ua.UserId == Guid.Parse(userId) && ua.AvatarId == avatarId);

            if (userAvatar == null)
            {
                return new ResultWithDataDto<string>(false, null, "Avatar not found");
            }

            var avatar = userAvatar.Avatar;

            var user = await _context.Users.FindAsync(Guid.Parse(userId));

            user.ProfilePictureUrl = avatar.ImagePath;

            await _context.SaveChangesAsync();

            return new ResultWithDataDto<string>(true, "Profile picture set successfully", null);
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

    }
}
