using Microsoft.Maui.Controls;

namespace FlowZone
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
		
		}

		private async void OnSignOutTapped(object sender, EventArgs e)
		{
			// Add your logic here for handling the skip button click event
			await DisplayAlert("Logout", "Logout button clicked!", "OK");
			//await Navigation.PushAsync(new GetStarted());
		}
	}
}
