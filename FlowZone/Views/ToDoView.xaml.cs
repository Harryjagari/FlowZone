using Microsoft.Maui.Controls;
using FlowZone.ViewModels;

namespace FlowZone.Views
{
    public partial class ToDoView : ContentPage
    {
        public ToDoView()
        {
            InitializeComponent();
            BindingContext = new ToDoViewModel(); // Set ToDoViewModel as the binding context
        }
    }
}
