using CommunityToolkit.Maui.Views;
using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class Profile : ContentPage
{
    private readonly ProfileViewModel _profileViewModel;

    public Profile(ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        _profileViewModel = profileViewModel;
        BindingContext = _profileViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _profileViewModel.InitializeAsync();
    }

    private async void SetProfileButton(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is UserAvatarDto profileDto)
        {
            if (BindingContext is ProfileViewModel viewModel)
            {
                await viewModel.SetProfileAsync(profileDto.AvatarId);
            }
        }
    }

    private async void OnResetTapped(object sender, EventArgs e)
    {
        var popup = new ResetPassword();
        Shell.Current.CurrentPage.ShowPopup(popup);
    }

}