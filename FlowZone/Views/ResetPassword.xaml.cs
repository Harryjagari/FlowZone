using CommunityToolkit.Maui.Views;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class ResetPassword : Popup
{
    private readonly UserViewModel _userViewModel;

    public ResetPassword()
    {
        InitializeComponent();
    }

    public ResetPassword(UserViewModel userViewModel)
    {
        InitializeComponent();
        _userViewModel = userViewModel;
        BindingContext = _userViewModel;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}