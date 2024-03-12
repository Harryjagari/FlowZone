using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class Register : ContentPage
{
	public Register(AuthViewModel authViewModel)
	{
		InitializeComponent();
		BindingContext = authViewModel;
	}
	private async void OnSignInTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Login));
	}
}