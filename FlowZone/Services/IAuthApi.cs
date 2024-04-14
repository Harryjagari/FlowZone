using FlowZone.shared.Dtos;
using Refit;

namespace FlowZone.Services
{
    public interface IAuthApi
	{
		[Post("/api/Register")]
		Task<ResultWithDataDto<AuthResponseDto>> SignupAsync(SignupRequestDto dto);

		[Post("/api/Login")]
		Task<ResultWithDataDto<AuthResponseDto>> SigninAsync(SigninRequestDto dto);
	}
}
