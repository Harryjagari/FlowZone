using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using System.Collections.ObjectModel;

namespace FlowZone.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly IProfileApi _profileApi;
        private readonly IAvatarApi _avatarApi;
        private readonly AuthService _authService;
        private readonly IPasswordApi _passwordApi;

        public ProfileViewModel(IProfileApi profileApi, AuthService authService, IAvatarApi avatarApi, IPasswordApi passwordApi)
        {
            _avatarApi = avatarApi;
            _profileApi = profileApi;
            _authService = authService;
            _passwordApi = passwordApi;
        }

        [ObservableProperty]
        private IEnumerable<ProfileDto> _profile = Enumerable.Empty<ProfileDto>();


        [ObservableProperty]
        private IEnumerable<UserAvatarDto> _userAvatar = Enumerable.Empty<UserAvatarDto>();

        [ObservableProperty]
        private Guid _userId;

        [ObservableProperty]
        private string? _email;
        [ObservableProperty]
        private string? _newPassword;

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _isInitialized;

        public object ResetPasswordRequestDto { get; private set; }

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            await LoadProfile(true);
            await LoadUserAvatars(true);
        }

        private async Task LoadProfile(bool initialLoad)
        {
            if (initialLoad)
                IsBusy = true;
            else
                IsRefreshing = true;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var ProfileResponse = await _profileApi.GetProfile();
                if (ProfileResponse.IsSuccess)
                {
                    Profile = new ObservableCollection<ProfileDto>(ProfileResponse.Data);
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
        private async Task LoadProfile() => await LoadProfile(false);


        private async Task LoadUserAvatars(bool initialLoad)
        {
            if (initialLoad)
                IsBusy = true;
            else
                IsRefreshing = true;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var UserAvatarResponse = await _avatarApi.GetPurchasedAvatars();
                if (UserAvatarResponse.IsSuccess)
                {
                     UserAvatar = new ObservableCollection<UserAvatarDto>(UserAvatarResponse.Data);
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
        private async Task LoadUserAvatars() => await LoadUserAvatars(false);



        [RelayCommand]
        public async Task SetProfileAsync(Guid AvatarId)
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Not lOgged In");
            }
            IsBusy = true;
            try
            {
                var SetProfileResponse = await _profileApi.SetProfilePicture(AvatarId);
                if (SetProfileResponse.IsSuccess)
                {
                    // Optionally, you can reload avatars or perform any other action after a successful purchase
                    await ShowAlertAsync("Successfully set profile. ");

                }
                else
                {
                    await ShowAlertAsync("Failed to set profile. " + SetProfileResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while setting profile: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task ResetPasswordAsync()
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Not lOgged In");
            }
            IsBusy = true;
            try
            {
                var resetPasswordDto = new ResetPasswordRequestDto
                {
                    Email = Email,
                    NewPassword = NewPassword
                };
                var ResetResponse = await _passwordApi.ResetPasswordAsync(resetPasswordDto);
                if (ResetResponse.IsSuccess)
                {
                    // Optionally, you can reload avatars or perform any other action after a successful purchase
                    await ShowAlertAsync("Successfully set profile. ");

                }
                else
                {
                    await ShowAlertAsync("Failed to set profile. " + ResetResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while setting profile: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
