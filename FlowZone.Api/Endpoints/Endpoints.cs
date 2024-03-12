using FlowZone.Api.Services;
using FlowZone.shared.Dtos;

namespace FlowZone.Api.Endpoints
{
	public static class Endpoints
	{
		public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app) 
		{
			app.MapPost("/api/signup",
				async (SignupRequestDto dto, AuthService authService) =>
					TypedResults.Ok(await authService.SignupAsync(dto)));

			app.MapPost("/api/signin",
				async (SigninRequestDto dto, AuthService authService) =>
					TypedResults.Ok(await authService.SigninAsync(dto)));

			app.MapGet("/api/ToDos",
				async (ToDoService todoService) =>
				TypedResults.Ok(await todoService.GetToDosAsync()));
            app.MapPost("/api/Avatars",
                async (AvatarDto avatarDto, AvatarService avatarService) =>
                    TypedResults.Ok(await avatarService.SaveAvatarAsync(avatarDto)));

            return app;

		}
	}
}
