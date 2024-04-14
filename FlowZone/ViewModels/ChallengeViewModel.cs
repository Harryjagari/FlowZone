using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.ViewModels
{
    public partial class ChallengeViewModel: BaseViewModel
    {
        private readonly IChallengeApi _challengeApi;
        private readonly AuthService _authService;

        public ChallengeViewModel(IChallengeApi challengeApi, AuthService authService)
        {
            _challengeApi = challengeApi;
            _authService = authService;
        }

        [ObservableProperty]
        private IEnumerable<ChallengeDto> _challenge = Enumerable.Empty<ChallengeDto>();

        [ObservableProperty]
        private Guid _challengeId;

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            await LoadAllChallenges(true);
        }
        private async Task LoadAllChallenges(bool initialLoad)
        {
            if (initialLoad)
                IsBusy = true;
            else
                IsRefreshing = true;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var ChallengesResponse = await _challengeApi.GetChallenges();
                if (ChallengesResponse.IsSuccess)
                {
                   Challenge = new ObservableCollection<ChallengeDto>(ChallengesResponse.Data);
                }

            }
            catch (Exception ex)
            {
                await ShowAlertAsync(ex.Message);
            }
            finally
            {
                IsBusy = IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task LoadAllChallenges() => await LoadAllChallenges(false);



        [RelayCommand]
        public async Task JoinChallengesAsync(Guid ChallengeId)
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Not lOgged In");
            }
            IsBusy = true;
            try
            {
                var JoinChallengeResponse = await _challengeApi.JoinChallenges(ChallengeId);
                if (JoinChallengeResponse.IsSuccess)
                {
                    // Optionally, you can reload avatars or perform any other action after a successful purchase
                    await ShowAlertAsync("Successfully joined challenge. ");

                }
                else
                {
                    await ShowAlertAsync("Failed to join challenge. " + JoinChallengeResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while joining challenge: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
