using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using FlowZone.shared.Dtos;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserChallengeController : ControllerBase
    {
        private readonly DataContext _context;

        public UserChallengeController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("JoinChallenge/{ChallengeId}")]
        public async Task<ResultWithDataDto<string>> JoinChallenge(Guid ChallengeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingUserChallenge = await _context.UserChallenges
                .FirstOrDefaultAsync(uc => uc.UserId == Guid.Parse(userId) && uc.ChallengeId == ChallengeId);

            if (existingUserChallenge != null)
            {
                return new ResultWithDataDto<string>(false, null, "User has already joined this challenge.");
            }

            var userChallenge = new UserChallenge
            {
                UserChallengeId = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                ChallengeId = ChallengeId,
                CompletionStatus = false
            };

            _context.UserChallenges.Add(userChallenge);
            await _context.SaveChangesAsync();

            return new ResultWithDataDto<string>(true, "User joined the challenge successfully", null);
        }

        [HttpPut("CompleteChallenge/{userChallengeId}")]
        public async Task<ResultWithDataDto<string>> CompleteChallenge(Guid userChallengeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userChallenge = await _context.UserChallenges.FindAsync(userChallengeId);

            if (userChallenge == null)
            {
                return new ResultWithDataDto<string>(false, null, "User challenge not found.");
            }

            if (userChallenge.UserId != Guid.Parse(userId))
            {
                return new ResultWithDataDto<string>(false, null, "User is not authorized to complete this challenge.");
            }

            if (userChallenge.CompletionStatus)
            {
                return new ResultWithDataDto<string>(false, null, "Challenge is already completed.");
            }

            userChallenge.CompletionStatus = true;
            _context.UserChallenges.Update(userChallenge);


            var challenge = await _context.Challenges.FindAsync(userChallenge.ChallengeId);
            var user = await _context.Users.FindAsync(userChallenge.UserId);
            if (challenge != null && user != null)
            {
                user.EarnedPoints += challenge.Points; 
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();

            return new ResultWithDataDto<string>(true, "Challenge completed successfully", null);
        }


        [HttpGet("GetUncompletedChallenges")]
        public async Task<ResultWithDataDto<List<UserChallengeDto>>> GetUncompletedChallenges()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var uncompletedChallenges = await _context.UserChallenges
                .Where(uc => uc.UserId == Guid.Parse(userId) && !uc.CompletionStatus && uc.Challenge.EndDate > DateTime.Now)
                .Select(uc => new UserChallengeDto
                {
                    UserChallengeId = uc.UserChallengeId,
                    ChallengeId = uc.ChallengeId,
                    Title = uc.Challenge.Name,
                    Description = uc.Challenge.Description,
                    Points = uc.Challenge.Points,
                    EndDate = uc.Challenge.EndDate
                })
                .ToListAsync();

            return new ResultWithDataDto<List<UserChallengeDto>>(true, uncompletedChallenges, null);
        }
    }
}
