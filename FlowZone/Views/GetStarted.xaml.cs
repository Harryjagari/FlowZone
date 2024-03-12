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
	// Event handler for the skip button click event
	private async void OnSkipClicked(object sender, EventArgs e)
	{
		// Add your logic here for handling the skip button click event
		//await DisplayAlert("Skip Clicked", "Skip button clicked!", "OK");
		await Shell.Current.GoToAsync(nameof(Login));
	}
}