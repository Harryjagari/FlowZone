using FlowZone.Services;

namespace FlowZone.Views;

public partial class GetStarted : ContentPage
{
	private readonly AuthService _authService;
	public GetStarted(AuthService authService)
	{
		InitializeComponent();
		_authService = authService;
	}

	protected async override void OnAppearing()
	{
		if(_authService.User is not null
			&& _authService.User.UserId !=default
			&& !string.IsNullOrWhiteSpace(_authService.Token))
		{
			await Shell.Current.GoToAsync($"//{nameof(Home)}");
		}
	}

	private async void OnSkipClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Login));
	}
}