using CommunityToolkit.Maui.Views;
using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class ForgetPassword : Popup
{
    private readonly UserViewModel _userViewModel;

    public ForgetPassword()
    {
        InitializeComponent();
    }

    public ForgetPassword(UserViewModel userViewModel)
    {
        InitializeComponent();
        _userViewModel = userViewModel;
        BindingContext = _userViewModel;
    }


    private async void OnCancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void OnSubmitTapped(object sender, EventArgs e)
    {
        if (BindingContext is UserViewModel viewModel)
        {
            await viewModel.SendOtpAsync();
        }
    }


}