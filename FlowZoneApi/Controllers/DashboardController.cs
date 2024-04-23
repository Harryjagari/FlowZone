using FlowZoneApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DataContext _context;

        public DashboardController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("user-count")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var userCount = await _context.Users.CountAsync();
            return Ok(userCount);
        }


        [HttpGet("avatar-count")]
        public async Task<ActionResult<int>> GetAvatarCount()
        {
            var avatarCount = await _context.Avatars.CountAsync();
            return Ok(avatarCount);
        }

        [HttpGet("challenge-count")]
        public async Task<ActionResult<int>> GetChallengeCount()
        {
            var challengeCount = await _context.Challenges.CountAsync();
            return Ok(challengeCount);
        }

        [HttpGet("challenges-joined-by-user/{year}/{month}")]
        public async Task<ActionResult<int[]>> GetChallengesJoinedByUser(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var data = await _context.UserChallenges
                .Where(cp => cp.JoinDate >= startDate && cp.JoinDate <= endDate)
                .GroupBy(cp => cp.UserId)
                .Select(g => g.Count())
                .ToArrayAsync();

            return Ok(data);
        }

        [HttpGet("avatars-purchased-in-month/{year}/{month}")]
        public async Task<ActionResult<int>> GetAvatarsPurchasedInMonth(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var avatarCount = await _context.UserAvatars
                .CountAsync(ua => ua.PurchaseDate >= startDate && ua.PurchaseDate <= endDate);

            return Ok(avatarCount);
        }
    }
}
