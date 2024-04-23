using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using FlowZoneApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PasswordService _passwordService;
        private readonly EmailService _emailService;

        public UserController(DataContext context, PasswordService passwordService, EmailService emailService)
        {
            _context = context;
            _passwordService = passwordService;
            _emailService = emailService;
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<ResultDto> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (dbUser == null)
            {
                return new ResultDto(false, "User does not exist");
            }

            var newPasswordHash = _passwordService.GenerateSaltAndHash(dto.NewPassword);
            dbUser.Hash = newPasswordHash.hashedPassword;
            dbUser.Salt = newPasswordHash.salt;

            await _context.SaveChangesAsync();

            return new ResultDto(true, "Password reset successfully");
        }


        [HttpPost("forget-password")]
        public async Task<ResultDto> ForgetPasswordAsync(ForgetPasswordRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {

                return new ResultDto(true, "If the email exists in our system, we have sent a password reset link to it.");
            }

            var otp = GenerateOTP();

            await _emailService.SendPasswordResetEmailAsync(user.Email, otp);

            user.ResetPasswordOTP = otp;
            user.ResetPasswordOTPIssueTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ResultDto(true, "If the email exists in our system, we have sent a password reset link to it.");
        }

        [HttpPost("reset-passwordWithOTP")]
        public async Task<ResultDto> ResetPasswordAsync(ResetPasswordWithOTPDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
            {
                return new ResultDto(false, "Invalid request");
            }

            if (user.ResetPasswordOTPIssueTime != null)
            {

                if (user.ResetPasswordOTP != dto.OTP || user.ResetPasswordOTPIssueTime.Value.AddMinutes(15) < DateTime.UtcNow)
                {
                    return new ResultDto(false, "Invalid OTP or OTP has expired");
                }
            }
            else
            {
                return new ResultDto(false, "Invalid OTP or OTP has expired");
            }

            var newPasswordHash = _passwordService.GenerateSaltAndHash(dto.NewPassword);
            user.Hash = newPasswordHash.hashedPassword;
            user.Salt = newPasswordHash.salt;

            user.ResetPasswordOTP = null;
            user.ResetPasswordOTPIssueTime = null;

            await _context.SaveChangesAsync();

            return new ResultDto(true, "Password reset successfully");
        }



        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
