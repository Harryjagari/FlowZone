using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;
using Refit;
using FlowZone.Admin.Services;
using System.Net.Http;
using FlowZone.shared.Dtos;

namespace FlowZone.Admin
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();
			builder.Services.AddTransient<AvatarDto>();
#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}

    }
}
