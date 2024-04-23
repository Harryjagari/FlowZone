using FlowZone.ViewModels;
using FlowZone.shared.Dtos;

namespace FlowZone.Views;

public partial class Home : ContentPage
{
	private readonly HomeViewModel _homeViewModel;

    private readonly ToDoViewModel _toDoViewModel;

    public Home(HomeViewModel homeViewModel, ToDoViewModel toDoViewModel)
	{
		InitializeComponent();
		_homeViewModel = homeViewModel;       
        BindingContext = _homeViewModel;
        _toDoViewModel = toDoViewModel;
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
            await _toDoViewModel.CompleteToDoAsync(toDoDto.ToDoId);
        }
    }

    private async void Button2_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ToDoDto toDoDto)
        {
            await _toDoViewModel.DeleteToDoAsync(toDoDto.ToDoId);
        }
    }

    private async void OnProfileTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Profile));
    }

    private async void OnToDoSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var selectedToDoItem = (ToDoDto)e.CurrentSelection.FirstOrDefault();

            _homeViewModel.SelectedToDoItem = selectedToDoItem;

            var updateToDoPage = new UpdateToDo(_toDoViewModel, selectedToDoItem);
            await Navigation.PushAsync(updateToDoPage);

            ((CollectionView)sender).SelectedItem = null;
        }
    }






}