using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using FlowZoneApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegisterController(DataContext context, TokenService tokenService, PasswordService passwordService) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly TokenService _tokenService = tokenService;
        private readonly PasswordService _passwordService = passwordService;

        [HttpPost]
        public async Task<ResultWithDataDto<AuthResponseDto>> SignupAsync(SignupRequestDto dto)
        {
            try
            {
                if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email == dto.Email))
                {
                    return ResultWithDataDto<AuthResponseDto>.Failure("Email already exists");
                }

                var user = new User
                {
                    Email = dto.Email,
                    Address = dto.Address,
                    UserName = dto.Name,
                };

                (user.Salt, user.Hash) = _passwordService.GenerateSaltAndHash(dto.Password);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return GenerateAuthResponse(user);
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while saving the entity changes.";
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                }

                return ResultWithDataDto<AuthResponseDto>.Failure(errorMessage);
            }
        }


        private ResultWithDataDto<AuthResponseDto> GenerateAuthResponse(User user)
        {
            var loggedInUser = new LoggedInUser(user.UserId, user.UserName, user.Email, user.Address);
            var token = _tokenService.GenerateJwt(loggedInUser);

            var authResponse = new AuthResponseDto(loggedInUser, token);

            return ResultWithDataDto<AuthResponseDto>.Success(authResponse);
        }
    }
}
