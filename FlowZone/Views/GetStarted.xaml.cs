namespace FlowZone.Views;

public partial class GetStarted : ContentPage
{
	public GetStarted()
	{
		InitializeComponent();
	}

	// Event handler for the skip button click event
	private async void OnSkipClicked(object sender, EventArgs e)
	{
		// Add your logic here for handling the skip button click event
		//await DisplayAlert("Skip Clicked", "Skip button clicked!", "OK");
		await Navigation.PushAsync(new Login());
	}
}