using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using System.Collections.ObjectModel;

namespace FlowZone.ViewModels
{
    public partial class AvatarViewModel:BaseViewModel
    {
        private readonly IAvatarApi _avatarApi;
        private readonly AuthService _authService;

        public AvatarViewModel(IAvatarApi avatarApi,AuthService authService)
        {
            _avatarApi = avatarApi;
            _authService = authService;
        }

        [ObservableProperty]
        private IEnumerable<AvatarDto> _avatar = Enumerable.Empty<AvatarDto>();

        [ObservableProperty]
        private Guid _avatarId;

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            await LoadAllAvatars(true);
        }
        private async Task LoadAllAvatars(bool initialLoad)
        {
            if(initialLoad)
                IsBusy = true;
            else
                IsRefreshing =true;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var AvatarResponse = await _avatarApi.GetAvatars();
                if (AvatarResponse.IsSuccess) 
                {
                    Avatar = new ObservableCollection<AvatarDto>(AvatarResponse.Data);
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
        private async Task LoadAvatars() => await LoadAllAvatars(false);


        [RelayCommand]
        public async Task PurchaseAvatarAsync(Guid AvatarId)
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Not logged in");
                return;
            }

            // Show confirmation dialog
            bool purchaseConfirmed = await Shell.Current.DisplayAlert("Confirmation", "Are you sure you want to purchase this avatar?", "Yes", "No");

            if (!purchaseConfirmed)
                return;

            IsBusy = true;
            try
            {
                var AvatarPurchaseResponse = await _avatarApi.PurchaseAvatar(AvatarId);
                if (AvatarPurchaseResponse.IsSuccess)
                {
                    await ShowAlertAsync("Successfully purchased avatar.");
                }
                else
                {
                    await ShowAlertAsync("Failed to purchase avatar. " + AvatarPurchaseResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while purchasing avatar: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
