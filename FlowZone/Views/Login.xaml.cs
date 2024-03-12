using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class Login : ContentPage
{
	public Login(AuthViewModel authViewModel )
	{
		InitializeComponent();
		BindingContext = authViewModel;
	}

	private async void OnSignUpTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Register));
	}

	private async void OnForgotPasswordTapped(object sender, EventArgs e)
	{
		// Add your logic here for handling the skip button click event
		await DisplayAlert("Forget password", "Forget password button clicked!", "OK");
		//await Navigation.PushAsync(new GetStarted());
	}
}