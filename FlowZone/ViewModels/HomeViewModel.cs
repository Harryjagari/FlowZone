using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class HomeViewModel(IToDosApi toDosApi, AuthService authService): BaseViewModel
    {
		private readonly IToDosApi _toDosApi = toDosApi;
		private readonly AuthService _authService = authService;
		[ObservableProperty]
        private ToDoDto[] _toDos = [];

        [ObservableProperty]
        private string _userName = string.Empty;

        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            UserName = _authService.User!.UserName;
            if (_isInitialized) return;
             
            IsBusy = true;
            try
            {
                _isInitialized = true;
               ToDos = await _toDosApi.GetToDosAsync();
				

			}
            catch (Exception ex) 
            {
                _isInitialized = false;
				await ShowErrorAlertAsync(ex.Message);
			}
            finally
            {
                IsBusy = false;
            }

        }
    }
}
