namespace FlowZone.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}
	private async void OnLoginButtonClicked(object sender, EventArgs e)
	{
		// Add your logic here for handling the skip button click event
		//await DisplayAlert("Login", "Login button clicked!", "OK");
		await Navigation.PushAsync(new MainPage());
	}

	private async void OnSignUpTapped(object sender, EventArgs e)
	{
		// Add your logic here for handling the skip button click event
		//await DisplayAlert("Sign up", "Sign up button clicked!", "OK");
		await Navigation.PushAsync(new Register());
	}

	private async void OnForgotPasswordTapped(object sender, EventArgs e)
	{
		// Add your logic here for handling the skip button click event
		await DisplayAlert("Forget password", "Forget password button clicked!", "OK");
		//await Navigation.PushAsync(new GetStarted());
	}
}