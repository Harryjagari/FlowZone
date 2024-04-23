using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using FlowZone.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.ViewModels
{
    public partial class HomeViewModel: BaseViewModel
    {
        private readonly IToDosApi _toDosApi;
        private readonly IProfileApi _profileApi;
        private readonly AuthService _authService;

        public HomeViewModel(IProfileApi profileApi, IToDosApi toDosApi, AuthService authService)
        {
            _toDosApi = toDosApi;
            _profileApi = profileApi;
            _authService = authService;
        }

        [ObservableProperty]
        private IEnumerable<ProfileDto> _profile = Enumerable.Empty<ProfileDto>();

        [ObservableProperty]
        private IEnumerable<ToDoDto> _toDos = Enumerable.Empty<ToDoDto>();

        private ToDoDto _selectedToDoItem;
        public ToDoDto SelectedToDoItem
        {
            get => _selectedToDoItem;
            set => SetProperty(ref _selectedToDoItem, value);
        }

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _isInitialized;

        [RelayCommand]
        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            IsRefreshing = true;

            await LoadProfile(true);
            await LoadToDos(true);

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


        private async Task LoadToDos(bool initialLoad)
        {
            if (initialLoad)
                IsBusy = true;
            else
                IsRefreshing = true;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var HomeTodoResponse = await _toDosApi.GetNearestToDoItems();
                if (HomeTodoResponse.IsSuccess)
                {
                    ToDos = new ObservableCollection<ToDoDto>(HomeTodoResponse.Data);
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
        private async Task LoadToDos() => await LoadToDos(false);
    }
}
