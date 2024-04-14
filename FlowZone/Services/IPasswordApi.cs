using FlowZone.shared.Dtos;
using Refit;

namespace FlowZone.Services
{
    public interface IPasswordApi
    {

        [Post("/api/User/forget-password")]
        Task<ResultDto> ForgetPasswordAsync(ForgetPasswordRequestDto dto);

        [Post("/api/User/reset-passwordWithOTP")]
        Task<ResultDto> ResetPasswordWithOTPAsync(ResetPasswordWithOTPDto dto);

        [Post("/api/User/reset-password")]
        [Headers("Authorization: Bearer")]
        Task<ResultDto> ResetPasswordAsync(ResetPasswordRequestDto dto);

    }
}
