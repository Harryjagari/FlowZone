using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views
{
    public partial class UpdateToDo : ContentPage
    {
        private readonly ToDoViewModel _toDoViewModel;

        private readonly ToDoDto _selectedToDoItem;

        public UpdateToDo(ToDoDto selectedToDoItem)
        {
            InitializeComponent();

            // Create a new instance of ToDoViewModel and pass selectedToDoItem
            _toDoViewModel = new ToDoViewModel();
            _selectedToDoItem = selectedToDoItem;

            // Set the properties of the ToDoViewModel based on selectedToDoItem
            _toDoViewModel.Title = selectedToDoItem.Title;
            _toDoViewModel.Description = selectedToDoItem.Description;
            _toDoViewModel.DueDate = selectedToDoItem.DueDate;
            _toDoViewModel.Priority = selectedToDoItem.Priority;

            // Set the BindingContext to the ToDoViewModel
            BindingContext = _toDoViewModel;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // You might want to initialize or load data here based on the scenario
        }

        //private async void OnUpdateTapped(object sender, EventArgs e)
        //{

        //    // Perform the update logic using the ViewModel
        //    if (BindingContext is ToDoViewModel viewModel)
        //    {
        //        await viewModel.UpdateToDoItemAsync();
        //    }
        //    else
        //    {
        //        await DisplayAlert("Error", "Invalid binding context", "OK");
        //    }
        //}
        private async void OnUpdateTapped(object sender, EventArgs e)
        {
            // Ensure that the BindingContext is a ToDoViewModel instance
            if (BindingContext is ToDoViewModel viewModel)
            {
                // Check if the BindingContext is ToDoViewModel
                if (viewModel.SelectedToDoItem != null)
                {
                    // Get the ID of the selected ToDo item
                    Guid toDoId = viewModel.SelectedToDoItem.ToDoId;

                    // Call the UpdateToDoItemAsync method in the ViewModel with the ToDo ID
                    await viewModel.UpdateToDoItemAsync(toDoId);
                }
                else
                {
                    await DisplayAlert("Error", "No ToDo item selected", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid binding context", "OK");
            }
        }


    }
}
