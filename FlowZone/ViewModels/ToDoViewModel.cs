using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;
using FlowZone.Views;

namespace FlowZone.ViewModels
{
    public partial class ToDoViewModel : BaseViewModel
    {
        private readonly IToDosApi _toDosApi;
        private readonly AuthService _authService;

        public ToDoViewModel()
        {
            // Parameterless constructor
        }

        public ToDoViewModel(IToDosApi toDosApi, AuthService authService)
        {
            _toDosApi = toDosApi;
            _authService = authService;
        }

        [ObservableProperty]
        private IEnumerable<ToDoDto> _toDos = Enumerable.Empty<ToDoDto>();

        [ObservableProperty]
        private Guid _toDoId;

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

            // Check if a ToDo item is selected for editing
            if (_selectedToDoItem != null)
            {
                // Populate the UI with the details of the selected ToDo item
                Title = _selectedToDoItem.Title;
                Description = _selectedToDoItem.Description;
                DueDate = _selectedToDoItem.DueDate;
                Priority = _selectedToDoItem.Priority;
            }
        }

        //public async Task InitializeAsync()
        //{
        //    if (_isInitialized)
        //        return;
        //    _isInitialized = true;
        //    await LoadAllToDos(true);

        //}
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
                var CreateToDoDto = new CreateToDoDto(Title, Description, DueDate, Priority);

                var result = await _toDosApi.CreateToDoItem(CreateToDoDto);

                if (result.IsSuccess)
                {
                    await GoToAsync($"//{nameof(ToDoView)}", animate: true);

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
                // Ensure a ToDo item is selected for updating

                // Create a new CreateToDoDto object from the properties of the selected ToDo item
                var updateDto = new CreateToDoDto(Title, Description, DueDate, Priority);

                // Call the API to update the ToDo item
                var result = await _toDosApi.UpdateToDoItem(ToDoId, updateDto);

                if (result.IsSuccess)
                {
                    // Optionally, navigate back to the ToDo page or perform other actions
                    await GoToAsync($"//{nameof(ToDo)}", animate: true);
                }
                else
                {
                    await ShowErrorAlertAsync(result.ErrorMessage ?? "Unknown Error in Updating");
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
                    // Optionally, you can reload avatars or perform any other action after a successful purchase
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
                await ShowAlertAsync("Not lOgged In");
            }
            IsBusy = true;
            try
            {
                var DeleteToDoResponse = await _toDosApi.DeleteToDoItem(ToDoId);
                if (DeleteToDoResponse.IsSuccess)
                {
                    // Optionally, you can reload avatars or perform any other action after a successful purchase
                    await ShowAlertAsync("Successfully deleted ToDo. ");

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
