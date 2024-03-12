using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.ViewModels
{
	public partial class BaseViewModel: ObservableObject
	{
		[ObservableProperty]
		private bool _isBusy;


		protected async Task GoToAsync(string url, bool animate = false) =>
			await Shell.Current.GoToAsync(url, animate);

		protected async Task ShowErrorAlertAsync(string errorMessage) =>
			await Shell.Current.DisplayAlert("Error", errorMessage, "OK");

		protected async Task ShowAlertAsync(string message) =>
			await Shell.Current.DisplayAlert("Alert", message, "OK");

	}
}
