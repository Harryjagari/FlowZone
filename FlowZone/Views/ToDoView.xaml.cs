using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views
{
    public partial class ToDoView : ContentPage
    {
        private readonly ToDoViewModel _toDoViewModel;

        public ToDoView()
        {
            InitializeComponent();
        }

        public ToDoView(ToDoViewModel toDoViewModel)
        {
            InitializeComponent();
            _toDoViewModel = toDoViewModel;
            BindingContext = _toDoViewModel;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _toDoViewModel.InitializeAsync();
        }


        private async void Button1_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ToDoDto toDoDto)
            {
                if (BindingContext is ToDoViewModel viewModel)
                {
                    await viewModel.CompleteToDoAsync(toDoDto.ToDoId);
                }
            }
        }

        private async void Button2_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ToDoDto toDoDto)
            {
                if (BindingContext is ToDoViewModel viewModel)
                {
                    await viewModel.DeleteToDoAsync(toDoDto.ToDoId);
                }
            }
        }

        private async void OnToDoSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                // Get the selected ToDo item
                var selectedToDoItem = (ToDoDto)e.CurrentSelection.FirstOrDefault();

                // Navigate to the UpdateToDo page and pass the selected ToDoDto item
                await Navigation.PushAsync(new UpdateToDo(selectedToDoItem));

                // Clear the selection to prevent multiple taps triggering this event
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}
