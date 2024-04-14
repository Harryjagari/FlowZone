using FlowZone.ViewModels;
using FlowZone.shared.Dtos;

namespace FlowZone.Views;

public partial class Home : ContentPage
{
	private readonly HomeViewModel _homeViewModel;

    public Home(HomeViewModel homeViewModel)
	{
		InitializeComponent();
		_homeViewModel = homeViewModel;
		BindingContext = _homeViewModel;
    }

	protected override async void OnAppearing()
	{
        base.OnAppearing();
        await _homeViewModel.InitializeAsync();
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

    private async void OnProfileTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Profile));
    }

    //private async void OnUpdateTapped(object sender, ItemTappedEventArgs e)
    //{
    //    var ToDo = (ToDoDto)e.Item;
    //    await Navigation.PushModalAsync(new UpdateToDo(ToDo)
    //}

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