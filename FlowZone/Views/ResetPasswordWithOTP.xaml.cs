using CommunityToolkit.Maui.Views;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class ResetPasswordWithOTP : Popup
{
    private readonly UserViewModel _userViewModel;

    public ResetPasswordWithOTP()
    {
        InitializeComponent();
    }
    public ResetPasswordWithOTP(UserViewModel userViewModel)
    {
        InitializeComponent();
        _userViewModel = userViewModel;
        BindingContext = _userViewModel;
    }
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnResetClicked(object sender, EventArgs e)
    {

    }
}