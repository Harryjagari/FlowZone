using FlowZone.Api.Data;
using FlowZone.Api.Data.Entities;
using FlowZone.shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FlowZone.Api.Services
{
	public class AuthService(DataContext context, TokenService tokenService,PasswordService passwordService)
	{
		private readonly DataContext _context=context;
		private readonly TokenService _tokenService = tokenService;
		private readonly PasswordService _passwordService = passwordService;
		public async Task<ResultWithDataDto<AuthResponseDto>> SignupAsync(SignupRequestDto dto)
		{
			if(await _context.Users.AsNoTracking().AnyAsync(u=>u.Email == dto.Email))
			{
				return ResultWithDataDto<AuthResponseDto>.Failure("Email already exist");
			}

			var user = new User
			{
				Email = dto.Email,
				Address = dto.Address,
				UserName = dto.Name,
			};

			(user.Salt,user.Hash) =_passwordService.GenerateSaltAndHash(dto.Password);

			try
			{
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();
				return GenerateAuthResponse(user);

			}
			catch (Exception ex) 
			{
				return ResultWithDataDto<AuthResponseDto>.Failure(ex.Message);
			}
			
		}



		public async Task<ResultWithDataDto<AuthResponseDto>> SigninAsync(SigninRequestDto dto)
		{
			var dbUser = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Email == dto.Email);
			if (dbUser is null) 
			{
				return ResultWithDataDto<AuthResponseDto>.Failure("User does not Exist");
			}

			if (!_passwordService.IsEqual(dto.Password, dbUser.Salt, dbUser.Hash))
				return ResultWithDataDto<AuthResponseDto>.Failure("Incorrect password");
			return GenerateAuthResponse(dbUser);
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
