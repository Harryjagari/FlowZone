using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class Home : ContentPage
{
	private readonly HomeViewModel _homeViewModel;


	public Home(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		_homeViewModel = homeViewModel;
		BindingContext = _homeViewModel;
	}

	protected override async void OnAppearing()
	{
		await _homeViewModel.InitializeAsync();
	}
}