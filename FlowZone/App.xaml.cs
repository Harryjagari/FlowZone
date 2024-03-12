using FlowZone.Services;
using FlowZone.Views;
namespace FlowZone
{
	public partial class App : Application
	{
		public App(AuthService authService)
		{
			InitializeComponent();

			authService.Initialize();

			MainPage = new AppShell(authService);
		}
	}
}
