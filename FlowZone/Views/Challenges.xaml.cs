using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class Challenges : ContentPage
{

    private readonly ChallengeViewModel _challengeViewModel;

    public Challenges(ChallengeViewModel challengeViewModel)
    {
        InitializeComponent();
        _challengeViewModel = challengeViewModel;
        BindingContext = _challengeViewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _challengeViewModel.InitializeAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ChallengeDto challengeDto)
        {
            if (BindingContext is ChallengeViewModel viewModel)
            {
                await viewModel.JoinChallengesAsync(challengeDto.ChallengeId);
            }
        }
    }
}