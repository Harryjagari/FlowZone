using FlowZone.shared.Dtos;
using FlowZone.ViewModels;
using Microsoft.VisualBasic;

namespace FlowZone.Views
{
    public partial class UpdateToDo : ContentPage
    {
        private readonly ToDoViewModel _toDoViewModel;

        private readonly ToDoDto _selectedToDoItem;

        public UpdateToDo(ToDoViewModel toDoViewModel,ToDoDto selectedToDoItem)
        {
            InitializeComponent();

            _toDoViewModel = toDoViewModel;
            _selectedToDoItem = selectedToDoItem;

            _toDoViewModel.Title = selectedToDoItem.Title;
            _toDoViewModel.Description = selectedToDoItem.Description;
            _toDoViewModel.DueDate = selectedToDoItem.DueDate;
            _toDoViewModel.Priority = selectedToDoItem.Priority;

            BindingContext = _toDoViewModel;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void OnUpdateTapped(object sender, EventArgs e)
        {
            if (BindingContext is ToDoViewModel viewModel)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(Title))
                    {
                        await DisplayAlert("Error","Title is required","Ok");
                        return;
                    }

                    viewModel.SelectedToDoItem = _selectedToDoItem;

                    await viewModel.UpdateToDoItemAsync(_selectedToDoItem.ToDoId);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to update ToDo item: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid binding context", "OK");
            }
        }




    }
}
