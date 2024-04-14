using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            // Get the authenticated user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                // Return unauthorized result
                return new ResultWithDataDto<List<UserAvatarDto>>(false, null, "Unauthorized");
            }

            // Retrieve all avatars purchased by the user and project name, price, and image URL
            var purchasedAvatars = _context.UserAvatars
                .Where(ua => ua.UserId == Guid.Parse(userId))
                .Select(ua => new UserAvatarDto
                {
                    AvatarId = ua.Avatar.AvatarId,
                    avatarName = ua.Avatar.Name,
                    avatarPrice = ua.Avatar.Price,
                    ImagePath = GetImagePath(ua.Avatar.ImagePath) // Retrieve image path
                })
                .ToList();

            return new ResultWithDataDto<List<UserAvatarDto>>(true, purchasedAvatars, null);
        }


        //[HttpGet("user")]
        //[ProducesResponseType(200, Type = typeof(List<AvatarDto>))]
        //[ProducesResponseType(401)]
        //public async Task<ResultWithDataDto<List<AvatarDto>>> GetPurchasedAvatars()
        //{
        //    // Get the authenticated user's ID
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }

        //    // Retrieve all avatars purchased by the user and project name, price, and image URL
        //    var purchasedAvatars = _context.UserAvatars
        //        .Where(ua => ua.UserId == Guid.Parse(userId))
        //        .Select(ua => new AvatarDto
        //        {
        //            AvatarId = ua.Avatar.AvatarId,
        //            avatarName = ua.Avatar.Name,
        //            avatarPrice = ua.Avatar.Price,
        //            ImagePath = GetImagePath(ua.Avatar.ImagePath) // Retrieve image path
        //        })
        //        .ToList();

        //    return Ok(purchasedAvatars);
        //}

        // Helper method to get full image path
        private static string GetImagePath(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return null;
            }

            // Assuming images are stored in the wwwroot/Images directory
            string imageName = Path.GetFileName(imagePath);
            return $"{imageName}";
        }

        [HttpGet("image/{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            // Assuming images are stored in the wwwroot/Images directory
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);

            if (System.IO.File.Exists(imagePath))
            {
                // Read the image file and return as a FileResult
                var imageFileStream = System.IO.File.OpenRead(imagePath);
                return File(imageFileStream, "image/jpeg"); // Adjust content type based on image type
            }
            else
            {
                // Return a placeholder image or an error response
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
            // Get the authenticated user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return new ResultWithDataDto<string>(false, null, "Unauthorized");
            }

            // Find the avatar by its ID
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


        //[HttpPost("{avatarId}")]
        //[ProducesResponseType(200)] 
        //[ProducesResponseType(400)]
        //[ProducesResponseType(401)]
        //[ProducesResponseType(404)]

        //public async Task<IActionResult> PurchaseAvatar(Guid avatarId)
        //{
        //    // Get the authenticated user's ID
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }

        //    // Find the avatar by its ID
        //    var avatar = await _context.Avatars.FindAsync(avatarId);
        //    if (avatar == null)
        //    {
        //        return NotFound("Avatar not found");
        //    }

        //    if (!UserHasEnoughCredits(userId, avatar.Price))
        //    {
        //        return BadRequest("Insufficient credits");
        //    }

        //    DeductCreditsFromUser(userId, avatar.Price);

        //    var userAvatar = new UserAvatar
        //    {
        //        UserId = Guid.Parse(userId),
        //        AvatarId = avatarId
        //    };

        //    _context.UserAvatars.Add(userAvatar);
        //    await _context.SaveChangesAsync();

        //    return Ok("Avatar purchased successfully");
        //}

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
