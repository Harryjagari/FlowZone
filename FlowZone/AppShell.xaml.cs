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
                typeof(ToDo),
				typeof(Pomodoro),
				typeof(MyChallenges),
				typeof(Challenges),
				typeof (UpdateToDo),
				typeof(ResetPassword),
				typeof(ToDoView),
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
			_authService.Signout();
			await Shell.Current.GoToAsync($"//{nameof(GetStarted)}");
		}

		private async void OnTodoTapped(object sender, EventArgs e)
		{
            FlyoutIsPresented = false;
            await Shell.Current.GoToAsync(nameof(ToDoView));
		}

		private async void OnPomodoroTapped(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;
            await Shell.Current.GoToAsync(nameof(Pomodoro));
        }

        private async void OnMyChallengesTapped(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;
            await Shell.Current.GoToAsync(nameof(MyChallenges));
        }

        private async void OnChallengeTapped(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;
            await Shell.Current.GoToAsync(nameof(Challenges));
        }

    }
}
