using CommunityToolkit.Maui.Views;
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
        var popup = new ForgetPassword();
        Shell.Current.CurrentPage.ShowPopup(popup);
    }

}