using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class Avatars : ContentPage
{
    private readonly AvatarViewModel _avatarViewModel;

    public Avatars(AvatarViewModel avatarViewModel)
	{
        InitializeComponent();
        _avatarViewModel = avatarViewModel;
        BindingContext = _avatarViewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _avatarViewModel.InitializeAsync();
    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is AvatarDto avatarDto)
        {
            if (BindingContext is AvatarViewModel viewModel)
            {
                await viewModel.PurchaseAvatarAsync(avatarDto.AvatarId);
            }
        }
    }
}