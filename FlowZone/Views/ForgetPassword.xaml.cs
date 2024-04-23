using CommunityToolkit.Maui.Views;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using FlowZone.ViewModels;
using System.Diagnostics;

namespace FlowZone.Views;

public partial class ForgetPassword : Popup
{
    private readonly UserViewModel _userViewModel;

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
            try
            {
                if (!string.IsNullOrEmpty(viewModel.SendEmail))
                {
                    await viewModel.SendOtpAsync(viewModel.SendEmail);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Email is required", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Error","Binding Context", "OK");
        }
    }

}