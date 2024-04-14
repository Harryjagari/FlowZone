using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using System.Collections.ObjectModel;

namespace FlowZone.ViewModels
{
    public partial class UserChallengesViewModel: BaseViewModel
    {
        private readonly IChallengeApi _challengeApi;
        private readonly AuthService _authService;

        public UserChallengesViewModel(IChallengeApi challengeApi, AuthService authService)
        {
            _challengeApi = challengeApi;
            _authService = authService;
        }

        [ObservableProperty]
        private IEnumerable<UserChallengeDto> _userChallenge = Enumerable.Empty<UserChallengeDto>();

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            await LoadAllUserChallenges(true);
        }
        private async Task LoadAllUserChallenges(bool initialLoad)
        {
            if (initialLoad)
                IsBusy = true;
            else
                IsRefreshing = true;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var ChallengesResponse = await _challengeApi.GetUserChallenges();
                if (ChallengesResponse.IsSuccess)
                {
                    UserChallenge = new ObservableCollection<UserChallengeDto>(ChallengesResponse.Data);
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
        private async Task LoadAllUserChallenges() => await LoadAllUserChallenges(false);


        [RelayCommand]
        public async Task CompleteChallengeAsync(Guid userChallengeId)
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Not lOgged In");
            }
            IsBusy = true;
            try
            {
                var CompleteChallengeResponse = await _challengeApi.CompleteChallenge(userChallengeId);
                if (CompleteChallengeResponse.IsSuccess)
                {
                    // Optionally, you can reload avatars or perform any other action after a successful purchase
                    await ShowAlertAsync("Successfully completed challenge. ");

                }
                else
                {
                    await ShowAlertAsync("Failed to complete challenge. " + CompleteChallengeResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while complteing challenge: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
