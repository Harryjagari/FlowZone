// ToDoViewModel class
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowZone.Services;
using FlowZone.shared.Dtos;

namespace FlowZone.ViewModels
{
    public partial class ToDoViewModel(IToDosApi toDosApi, AuthService authService) : BaseViewModel
    {
        private readonly IToDosApi _toDosApi = toDosApi;
        private readonly AuthService _authService = authService;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private DateTime _created;

        [ObservableProperty]
        private DateTime _dueDate;

        [ObservableProperty]
        private string _priority;
        public bool CanCreate => !string.IsNullOrEmpty(Title)
                && !string.IsNullOrEmpty(Description)
                && Created != default(DateTime)
                && DueDate != default(DateTime)
                && !string.IsNullOrEmpty(Priority);


        [RelayCommand]
        private async Task CreateToDoAsync()
        {
            IsBusy = true;
            try
            {
                // Ensure all required fields are filled
                if (CanCreate)
                {
                    var toDoDto = new ToDoDto(
                        Guid.NewGuid(), // Generate a new Guid for ToDoId
                        Title,
                        Description,
                        DateTime.Now, // Assuming Created is set to the current date and time
                        DueDate,
                        Priority
                    );

                    // Call the API to create ToDo item
                    var result = await _toDosApi.CreateToDoItem(toDoDto);

                    if (result.IsSuccess)
                    {
                        // Handle success, navigate to another page, refresh UI, etc.
                        await Shell.Current.DisplayAlert("Success", "ToDo item created successfully", "OK");
                    }
                    else
                    {
                        // Handle failure, show error message, etc.
                        await Shell.Current.DisplayAlert("Error", result.ErrorMessage ?? "Unknown Error", "OK");
                    }
                }
                else
                {
                    // Handle case where required fields are not filled
                    await Shell.Current.DisplayAlert("Error", "All fields are required", "OK");
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
