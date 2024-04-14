using FlowZone.shared.Dtos;
using FlowZone.ViewModels;

namespace FlowZone.Views
{
    public partial class ToDo : ContentPage
    {
        private readonly ToDoViewModel _toDoViewModel;

        public ToDo(ToDoViewModel toDoViewModel)
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

    }
}
