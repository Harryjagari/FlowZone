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

        public UserViewModel(IPasswordApi passwordApi)
        {
            _passwordApi = passwordApi;
        }

        [ObservableProperty]
        private bool _isBusy;

        private string _sendEmail;
        public string SendEmail
        {
            get => _sendEmail;
            set => SetProperty(ref _sendEmail, value);
        }


        [ObservableProperty]
        private string _newPassword;

        [ObservableProperty]
        private string _otp;


        [RelayCommand]
        public async Task SendOtpAsync(string SendEmail)
        {
            IsBusy = true;
            try
            {
                if (!IsValidEmailFormat(SendEmail))
                {
                    await ShowAlertAsync("Invalid email format");
                    return;
                }

                var resetPasswordDto = new ForgetPasswordRequestDto
                {
                    Email = SendEmail
                };
                if (_passwordApi == null)
                {
                    await ShowAlertAsync("Password API is not initialized.");
                    return;
                }

                var ResetResponse = await _passwordApi.ForgetPasswordAsync(resetPasswordDto);
                if (ResetResponse.IsSuccess)
                {
                    await ShowAlertAsync("Otp sent successfully.");

                    var popup = new ResetPasswordWithOTP(this);
                    Shell.Current.CurrentPage.ShowPopup(popup);
                }
                else
                {
                    await ShowAlertAsync("Failed to send OTP. " + ResetResponse.ErrorMessage);
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

        private bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var parts = email.Split('@');
            if (parts.Length != 2)
                return false;

            if (parts[0].Length < 1 || parts[1].Length < 1)
                return false;

            if (!parts[1].Contains('.'))
                return false;

            return true;
        }


        [RelayCommand]
        public async Task ResetPasswordWithOtpAsync()
        {
            IsBusy = true;
            try
            {

                if (string.IsNullOrWhiteSpace(SendEmail))
                {
                    await ShowAlertAsync("Email is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Otp))
                {
                    await ShowAlertAsync("OTP is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    await ShowAlertAsync("New password is required");
                    return;
                }

                var resetPasswordDto = new ResetPasswordWithOTPDto
                {
                    Email = SendEmail,
                    OTP = Otp,
                    NewPassword = NewPassword
                };

                if (_passwordApi == null)
                {
                    await ShowAlertAsync("Password API is not initialized.");
                    return;
                }

                var ResetResponse = await _passwordApi.ResetPasswordWithOTPAsync(resetPasswordDto);
                if (ResetResponse.IsSuccess)
                {
                    await ShowAlertAsync("Successfully reset password.");
                }
                else
                {
                    await ShowAlertAsync("Failed to reset password. " + ResetResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while resetting password: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
