namespace FlowZone.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

	private async void OnRegisterButtonClicked(object sender, EventArgs e)
	{
		// Add your logic here for handling the skip button click event
		await DisplayAlert("Register", "Register button clicked!", "OK");
		//await Navigation.PushAsync(new GetStarted());
	}
}