using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using FlowZone.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.ViewModels
{
	public partial class AuthViewModel(IAuthApi authApi, AuthService authService, CommonService commonService) : BaseViewModel
	{

		private readonly IAuthApi _authApi=authApi;

		private readonly AuthService _authService=authService;

        private readonly CommonService _commonService = commonService;

        [ObservableProperty]
		private bool _isBusy;

		[ObservableProperty,NotifyPropertyChangedFor(nameof(CanSignup))]
		private string? _name;

		[ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignin)), NotifyPropertyChangedFor(nameof(CanSignup))]
		private string? _email;

		[ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignin)), NotifyPropertyChangedFor(nameof(CanSignup))]
		private string? _password;

		[ObservableProperty, NotifyPropertyChangedFor(nameof(CanSignup))]
		private string? _address;

		public bool CanSignin => !string.IsNullOrEmpty(Email)
						&& !string.IsNullOrEmpty(Password);

		public bool CanSignup => CanSignin
								&& !string.IsNullOrEmpty(Name)
								&& !string.IsNullOrEmpty(Address);

		[RelayCommand]
		private async Task SignupAsync()
		{
			IsBusy = true;
			try
			{
				var signupDto = new SignupRequestDto(Name, Email, Password, Address);

				var result = await _authApi.SignupAsync(signupDto);

				if(result.IsSuccess)
				{
					_authService.Signin(result.Data);
					await GoToAsync($"//{nameof(Login)}", animate: true);

				}
				else
				{
					await ShowErrorAlertAsync(result.ErrorMessage ?? "Unknown Error in Signing up");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		[RelayCommand]
		private async Task SigninAsync()
		{
			IsBusy = true;
			try
			{
				var signinDto = new SigninRequestDto(Email, Password);

				var result = await _authApi.SigninAsync(signinDto);

				if (result.IsSuccess)
				{
					_authService.Signin(result.Data);
					await GoToAsync($"//{nameof(Home)}", animate: true);

				}
				else
				{
					await ShowErrorAlertAsync(result.ErrorMessage ?? "Unknown Error in Signing in");
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
