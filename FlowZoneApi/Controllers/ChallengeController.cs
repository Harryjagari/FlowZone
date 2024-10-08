﻿using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly DataContext _context;

        public ChallengeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallengeDto>>> GetAllChallenges()
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

            return Ok(challenges);
        }


        [HttpPost]
        public async Task<ActionResult<ChallengeDto>> AddChallenge(ChallengeDto challengeDto)
        {
            var challenge = new Challenge
            {
                Name = challengeDto.Title,
                Description = challengeDto.Description,
                Points = challengeDto.Points,
                StartDate = challengeDto.StartDate,
                EndDate = challengeDto.EndDate,
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetAllChallenges), new { id = challenge.ChallengeId }, challengeDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChallenge(Guid id, ChallengeDto challengeDto)
        {
            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null)
            {
                return NotFound();
            }

            challenge.Name = challengeDto.Title;
            challenge.Description = challengeDto.Description;
            challenge.Points = challengeDto.Points;
            challenge.StartDate = challengeDto.StartDate;
            challenge.EndDate = challengeDto.EndDate;

            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeExists(id))
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



        [HttpGet("{id}")]
        public async Task<ActionResult<ChallengeDto>> GetChallengeById(Guid id)
        {
            var challenge = await _context.Challenges
                                        .Where(c => c.ChallengeId == id)
                                        .Select(c => new ChallengeDto
                                        {
                                            ChallengeId = c.ChallengeId,
                                            Title = c.Name,
                                            Description = c.Description,
                                            StartDate = c.StartDate,
                                            EndDate = c.EndDate
                                        })
                                        .FirstOrDefaultAsync();

            if (challenge == null)
            {
                return NotFound();
            }

            return challenge;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallenge(Guid id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            _context.Challenges.Remove(challenge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChallengeExists(Guid id)
        {
            return _context.Challenges.Any(e => e.ChallengeId == id);
        }
    }
}
