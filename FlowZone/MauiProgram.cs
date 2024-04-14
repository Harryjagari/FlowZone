using FlowZone.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;
using Refit;
using FlowZone.Services;
using FlowZone.ViewModels;
using FlowZone.shared;
using System.Net.Http;
using CommunityToolkit.Maui;







#if ANDROID
using Xamarin.Android.Net;
using System.Net.Security;
#endif
using System.Security.Cryptography.X509Certificates;

namespace FlowZone
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
#if ANDROID
			Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (h, v) =>
			{
				h.PlatformView.BackgroundTintList =
				Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
			});
#endif
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});
			
#if DEBUG
			builder.Logging.AddDebug();
#endif
			builder.Services.AddTransient<AuthViewModel>()
				.AddTransient<Register>()
				.AddTransient<Login>();

			builder.Services.AddSingleton<AuthService>();

			builder.Services.AddSingleton<CommonService>();

			builder.Services.AddTransient<GetStarted>();

			builder.Services.AddSingleton<HomeViewModel>()
				.AddSingleton<Home>();
            builder.Services.AddSingleton<AvatarViewModel>()
				.AddTransient<Avatars>();
            builder.Services.AddSingleton<ChallengeViewModel>()
                .AddTransient<Challenges>();
            builder.Services.AddSingleton<ToDoViewModel>()
                .AddTransient<ToDo>()
                .AddTransient<ToDoView>()
                .AddTransient<UpdateToDo>();
            builder.Services.AddSingleton<UserChallengesViewModel>()
                .AddTransient<MyChallenges>();
            builder.Services.AddSingleton<ProfileViewModel>()
                .AddTransient<Profile>()
                .AddTransient<ResetPassword>();

            builder.Services.AddSingleton<UserViewModel>()
                .AddTransient<ForgetPassword>()
                .AddTransient<ResetPasswordWithOTP>();

            ConfigureRefit(builder.Services);


			return builder.Build();
		}

		static void ConfigureRefit(IServiceCollection services)
		{
			services.AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IToDosApi>(sp =>
            {
				var commonService = sp.GetRequiredService<CommonService>();
                return new RefitSettings()
                {
                    AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(commonService.Token ?? string.Empty)
                };
            })
				.ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IAvatarApi>(sp =>
			{
                var commonService = sp.GetRequiredService<CommonService>();
                return new RefitSettings()
				{
                    AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(commonService.Token ?? string.Empty)
                };
			})
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IChallengeApi>(sp =>
            {
                var commonService = sp.GetRequiredService<CommonService>();
                return new RefitSettings()
                {
                    AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(commonService.Token ?? string.Empty)
                };
            })
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IPasswordApi>(sp =>
            {
                var commonService = sp.GetRequiredService<CommonService>();
                return new RefitSettings()
                {
                    AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(commonService.Token ?? string.Empty)
                };
            })
                .ConfigureHttpClient(SetHttpClient);

            services.AddRefitClient<IProfileApi>(sp =>
            {
                var commonService = sp.GetRequiredService<CommonService>();
                return new RefitSettings()
                {
                    AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(commonService.Token ?? string.Empty)
                };
            })
                .ConfigureHttpClient(SetHttpClient);

            static void SetHttpClient(HttpClient httpClient) =>
				httpClient.BaseAddress = new Uri(AppConstants.BaseApiUrl);

        }
	}
}
