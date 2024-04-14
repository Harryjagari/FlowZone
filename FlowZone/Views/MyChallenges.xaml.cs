using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class MyChallenges : ContentPage
{
    private readonly UserChallengesViewModel _userChallengeViewModel;

    public MyChallenges(UserChallengesViewModel userChallengeViewModel)
    {
        InitializeComponent();
        _userChallengeViewModel = userChallengeViewModel;
        BindingContext = _userChallengeViewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _userChallengeViewModel.InitializeAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is UserChallengeDto userChallengeDto)
        {
            if (BindingContext is UserChallengesViewModel viewModel)
            {
                await viewModel.CompleteChallengeAsync(userChallengeDto.UserChallengeId);
            }
        }
    }
}