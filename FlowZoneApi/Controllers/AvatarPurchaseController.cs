using FlowZone.shared;
using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace FlowZoneApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarPurchaseController : ControllerBase
    {
        private readonly DataContext _context;

        public AvatarPurchaseController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ResultWithDataDto<List<UserAvatarDto>>> GetPurchasedAvatars()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return new ResultWithDataDto<List<UserAvatarDto>>(false, null, "Unauthorized");
            }

            var purchasedAvatars = _context.UserAvatars
                .Where(ua => ua.UserId == Guid.Parse(userId))
                .Select(ua => new UserAvatarDto
                {
                    AvatarId = ua.Avatar.AvatarId,
                    avatarName = ua.Avatar.Name,
                    avatarPrice = ua.Avatar.Price,
                    ImagePath = GetImagePath(ua.Avatar.ImagePath) 
                })
                .ToList();

            return new ResultWithDataDto<List<UserAvatarDto>>(true, purchasedAvatars, null);
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


        [HttpPost("{avatarId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ResultWithDataDto<string>> PurchaseAvatar(Guid avatarId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return new ResultWithDataDto<string>(false, null, "Unauthorized");
            }

            var userIdGuid = new Guid(userId);

            var userHasAvatar = await _context.UserAvatars.AnyAsync(ua => ua.UserId == userIdGuid && ua.AvatarId == avatarId);
            if (userHasAvatar)
            {
                return new ResultWithDataDto<string>(false, null, "You have already purchased this avatar");
            }

            var avatar = await _context.Avatars.FindAsync(avatarId);
            if (avatar == null)
            {
                return new ResultWithDataDto<string>(false, null, "Avatar not found");
            }

            if (!UserHasEnoughCredits(userId, avatar.Price))
            {
                return new ResultWithDataDto<string>(false, null, "Insufficient credits");
            }

            DeductCreditsFromUser(userId, avatar.Price);

            var userAvatar = new UserAvatar
            {
                UserId = Guid.Parse(userId),
                AvatarId = avatarId,
                PurchaseDate = DateTime.Now
            };

            _context.UserAvatars.Add(userAvatar);
            await _context.SaveChangesAsync();

            return new ResultWithDataDto<string>(true, "Avatar purchased successfully", null);
        }



        private bool UserHasEnoughCredits(string userId, double Price)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == Guid.Parse(userId));
            if (user == null)
            {
                return false;
            }

            return user.EarnedPoints >= Price;
        }

        private void DeductCreditsFromUser(string userId, int Price)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == Guid.Parse(userId));
            if (user != null)
            {
                user.EarnedPoints -= Price;

                _context.SaveChanges();
            }
        }

    }
}
