using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using FlowZone.Views;

namespace FlowZone.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {
        private readonly IPasswordApi _passwordApi;

        //[ObservableProperty, NotifyPropertyChangedFor(nameof(CanSendOTP)), NotifyPropertyChangedFor(nameof(CanReset))]
        //private string? _sendEmail;



        //[ObservableProperty, NotifyPropertyChangedFor(nameof(CanReset))]
        //private string? _newPassword;

        //[ObservableProperty, NotifyPropertyChangedFor(nameof(CanReset))]
        //private string? _otp;
        [ObservableProperty]
        private bool _isBusy;

        public UserViewModel(IPasswordApi passwordApi)
        {
            _passwordApi = passwordApi;
        }

        public UserViewModel()
        {

        }


        private string? _sendEmail;
        public string? SendEmail
        {
            get => _sendEmail;
            set => SetProperty(ref _sendEmail, value);
        }


        [ObservableProperty]
        private string? _newPassword;

        [ObservableProperty]
        private string? _otp;

        //public bool CanSendOTP => !string.IsNullOrEmpty(SendEmail);
        //public bool CanReset => CanSendOTP && !string.IsNullOrEmpty(Otp) && !string.IsNullOrEmpty(NewPassword);

        [RelayCommand]
        public async Task SendOtpAsync()
        {
            IsBusy = true;
            try
            {
                var resetPasswordDto = new ForgetPasswordRequestDto
                {
                    Email = SendEmail
                };

                var ResetResponse = await _passwordApi.ForgetPasswordAsync(resetPasswordDto);
                if (ResetResponse.IsSuccess)
                {
                    await ShowAlertAsync("Otp sent successfully.");
                    var popup = new ResetPasswordWithOTP();
                    Shell.Current.CurrentPage.ShowPopup(popup);
                }
                else
                {
                    await ShowAlertAsync("Failed to sent OTP. " + ResetResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while sending OTP: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task ResetPasswordWithOtpAsync()
        {
            IsBusy = true;
            try
            {
                var resetPasswordDto = new ResetPasswordWithOTPDto
                {
                    Email = SendEmail,
                    OTP = Otp,
                    NewPassword = NewPassword
                };
                var ResetResponse = await _passwordApi.ResetPasswordWithOTPAsync(resetPasswordDto);
                if (ResetResponse.IsSuccess)
                {
                    await ShowAlertAsync("Successfully set profile.");
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
