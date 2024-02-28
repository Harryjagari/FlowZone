using FlowZone.Views;
namespace FlowZone
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
			// Navigate to the GetStartedPage instead of MainPage
			MainPage = new NavigationPage(new GetStarted());
		}
	}
}
