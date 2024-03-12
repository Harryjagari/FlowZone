using FlowZone.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;
using Refit;
using FlowZone.Services;
using FlowZone.ViewModels;




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

			builder.Services.AddTransient<GetStarted>();

			builder.Services.AddSingleton<HomeViewModel>()
				.AddSingleton<Home>();
			ConfigureRefit(builder.Services);

			return builder.Build();
		}

		private static void ConfigureRefit(IServiceCollection services)
		{
			var refitSettings = new RefitSettings
			{
				HttpMessageHandlerFactory = () =>
				{
#if ANDROID
					return new AndroidMessageHandler
					{
						ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, chain, sslPolicyErrors) =>
						{
							return certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None;							
						}
					};
#endif
					return null;
				}
			};

			services.AddRefitClient<IAuthApi>(refitSettings)
				.ConfigureHttpClient(SetHttpClient);

			services.AddRefitClient<IToDosApi>(refitSettings)
				.ConfigureHttpClient(SetHttpClient);

            static void SetHttpClient(HttpClient httpClient)
			{
				var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
						? "https://10.0.2.2:7061"
						: "https://localhost:7061";
				httpClient.BaseAddress = new Uri(baseUrl);
			}
		}
	}
}
