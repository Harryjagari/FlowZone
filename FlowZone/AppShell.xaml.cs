using FlowZone.Services;
using FlowZone.Views;
using Microsoft.Maui.Controls;

namespace FlowZone
{
	public partial class AppShell : Shell
	{
		private readonly AuthService _authService;
		public AppShell(AuthService authService)
		{
			InitializeComponent();
			RegisterRoutes();
			_authService = authService;
		}

		private readonly static Type[] _routablePageTypes =
			[
				typeof(Login),
				typeof(Register),
			];

		private static void RegisterRoutes()
		{
			foreach (var pageType in _routablePageTypes)
			{
				Routing.RegisterRoute(pageType.Name, pageType);
			}
		}

		private async void OnSignOutTapped(object sender, EventArgs e)
		{
			// Add your logic here for handling the skip button click event
			//await DisplayAlert("Logout", "Logout button clicked!", "OK");
			//await Navigation.PushAsync(new GetStarted());
			_authService.Signout();
			await Shell.Current.GoToAsync($"//{nameof(GetStarted)}");
		}
	}
}
