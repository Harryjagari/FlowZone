using FlowZone.shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
