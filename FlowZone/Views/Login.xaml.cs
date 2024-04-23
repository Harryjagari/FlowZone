using CommunityToolkit.Maui.Views;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class Login : ContentPage
{
    private readonly UserViewModel _userViewModel;
    private readonly AuthViewModel _authViewModel;

    public Login(AuthViewModel authViewModel, UserViewModel userViewModel)
    {
        InitializeComponent();
        BindingContext = authViewModel;
        _authViewModel = authViewModel;
        _userViewModel = userViewModel;
    }

    private async void OnSignUpTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Register));
	}

	private async void OnForgotPasswordTapped(object sender, EventArgs e)
	{
        var popup = new ForgetPassword(_userViewModel);
        Shell.Current.CurrentPage.ShowPopup(popup);
    }

}