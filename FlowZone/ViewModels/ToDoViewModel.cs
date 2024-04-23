using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using FlowZone.Views;
using Microsoft.AspNetCore.Mvc;

namespace FlowZone.ViewModels
{
    public partial class ToDoViewModel : BaseViewModel
    {
        private readonly IToDosApi _toDosApi;
        private readonly AuthService _authService;

        public ToDoViewModel(IToDosApi toDosApi, AuthService authService)
        {
            _toDosApi = toDosApi;
            _authService = authService;

        }

        [ObservableProperty]
        private IEnumerable<ToDoDto> _toDos = Enumerable.Empty<ToDoDto>();

        [ObservableProperty]
        private Guid _toDoId;

        public DateTime MinDate =>DateTime.Now;

        [ObservableProperty]
        private bool _isRefreshing;

        private bool _isInitialized;

        private ObservableCollection<CreateToDoDto> _toDoItems;
        public ObservableCollection<CreateToDoDto> ToDoItems
        {
            get => _toDoItems;
            set => SetProperty(ref _toDoItems, value);
        }

        private ToDoDto _selectedToDoItem;
        public ToDoDto SelectedToDoItem
        {
            get => _selectedToDoItem;
            set => SetProperty(ref _selectedToDoItem, value);
        }


        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanCreateToDo)), NotifyPropertyChangedFor(nameof(CanUpdateToDo))]
        private string? _title;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanCreateToDo)), NotifyPropertyChangedFor(nameof(CanUpdateToDo))]
        private string? _description;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanCreateToDo)), NotifyPropertyChangedFor(nameof(CanUpdateToDo))]
        private DateTime _dueDate;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(CanCreateToDo)), NotifyPropertyChangedFor(nameof(CanUpdateToDo))]
        private string? _priority;

        public bool CanCreateToDo => !string.IsNullOrEmpty(Title)
                        && !string.IsNullOrEmpty(Description)
                        && DueDate != default
                        && !string.IsNullOrEmpty(Priority);


        public bool CanUpdateToDo => CanCreateToDo;

        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            await LoadAllToDos(true);

            if (_selectedToDoItem != null)
            {
                Title = _selectedToDoItem.Title;
                Description = _selectedToDoItem.Description;
                DueDate = _selectedToDoItem.DueDate;
                Priority = _selectedToDoItem.Priority;
            }
        }


        private async Task LoadAllToDos(bool initialLoad)
        {
            if (initialLoad)
                IsBusy = true;
            else
                IsRefreshing = true;
            IsBusy = true;
            try
            {
                await Task.Delay(100);
                var ToDosResponse = await _toDosApi.GetAllToDoItems();
                if (ToDosResponse.IsSuccess)
                {
                    ToDos = new ObservableCollection<ToDoDto>(ToDosResponse.Data);
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
        private async Task LoadAllToDos() => await LoadAllToDos(false);

        [RelayCommand]
        public async Task CreateToDoItemAsync()
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Title))
                {
                    await ShowErrorAlertAsync("Title is required");
                    return;
                }

                if (DueDate == default(DateTime))
                {
                    await ShowErrorAlertAsync("Due date is required");
                    return;
                }

                var CreateToDoDto = new CreateToDoDto(Title, Description, DueDate, Priority);

                var result = await _toDosApi.CreateToDoItem(CreateToDoDto);

                if (result.IsSuccess)
                {
                    await GoToAsync(nameof(ToDoView), animate: true);
                }
                else
                {
                    await ShowErrorAlertAsync(result.ErrorMessage ?? "Unknown Error in Creating up");
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
        public async Task UpdateToDoItemAsync(Guid ToDoId)
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Title))
                {
                    await ShowErrorAlertAsync("Title is required");
                    return;
                }

                if (DueDate == default(DateTime))
                {
                    await ShowErrorAlertAsync("Due date is required");
                    return;
                }
                if (Title != null && Description != null && Priority != null)
                {
                    var updateDto = new CreateToDoDto(Title, Description, DueDate, Priority);

                    try
                    {
                        var result = await _toDosApi.UpdateToDoItem(ToDoId, updateDto);
                        if (result.IsSuccess)
                        {
                            await ShowAlertAsync("ToDo item successfully updated");

                            await GoToAsync($"//{nameof(Home)}", animate: true);
                        }
                        else
                        {
                            await ShowErrorAlertAsync(result.ErrorMessage ?? "Unknown Error in Updating");
                        }

                    }
                    catch(Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                    }


                }
                else
                {
                    await ShowAlertAsync("One or more properties is null");
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
        public async Task CompleteToDoAsync(Guid ToDoId)
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Not lOgged In");
            }
            IsBusy = true;
            try
            {
                var CompleteToDoResponse = await _toDosApi.CompleteToDo(ToDoId);
                if (CompleteToDoResponse.IsSuccess)
                {
                    await ShowAlertAsync("Successfully completed ToDo. ");

                }
                else
                {
                    await ShowAlertAsync("Failed to complete ToDo. " + CompleteToDoResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while complteing ToDo: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        public async Task DeleteToDoAsync(Guid ToDoId)
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Not logged in");
                return;
            }

            bool deleteConfirmed = await Shell.Current.DisplayAlert("Confirmation", "Are you sure you want to delete this ToDo?", "Yes", "No");

            if (!deleteConfirmed)
                return;

            IsBusy = true;
            try
            {
                var DeleteToDoResponse = await _toDosApi.DeleteToDoItem(ToDoId);
                if (DeleteToDoResponse.IsSuccess)
                {
                    await ShowAlertAsync("Successfully deleted ToDo.");
                }
                else
                {
                    await ShowAlertAsync("Failed to delete ToDo. " + DeleteToDoResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("An error occurred while deleting ToDo: " + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
