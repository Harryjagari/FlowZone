using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeMobileController : ControllerBase
    {
        private readonly DataContext _context;

        public ChallengeMobileController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Challenge
        [HttpGet]
        public async Task<ResultWithDataDto<List<ChallengeDto>>> GetAllChallenges()
        {
            var challenges = await _context.Challenges.Select(c => new ChallengeDto
            {
                ChallengeId = c.ChallengeId,
                Title = c.Name,
                Description = c.Description,
                Points = c.Points,
                StartDate = c.StartDate,
                EndDate = c.EndDate
            }).ToListAsync();

            var result = ResultWithDataDto<List<ChallengeDto>>.Success(challenges);

            return result;
        }
    }
}
