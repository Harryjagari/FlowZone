using FlowZone.ViewModels;

namespace FlowZone.Views;

public partial class ToDo : ContentPage
{
	public ToDo(ToDoViewModel toDoViewModel)
	{
		InitializeComponent();
        BindingContext = toDoViewModel;

    }
}