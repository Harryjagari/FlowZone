using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using FlowZone.shared.Dtos; 
using FlowZone.Helpers;
using Newtonsoft.Json; 
using System.Net.Http.Json;

namespace FlowZone.ViewModels
{
    public class ToDoViewModel : BindableObject
    {
        private const string BaseUrl = "https://localhost:7026/api/ToDo";

        private ObservableCollection<ToDoDto> _toDoItems;
        public ObservableCollection<ToDoDto> ToDoItems
        {
            get => _toDoItems;
            set
            {
                _toDoItems = value;
                OnPropertyChanged(nameof(ToDoItems));
            }
        }

        public ICommand LoadToDoItemsCommand { get; private set; }
        public ICommand AddToDoItemCommand { get; private set; }
        public ICommand UpdateToDoItemCommand { get; private set; }
        public ICommand DeleteToDoItemCommand { get; private set; }

        public ToDoViewModel()
        {
            LoadToDoItemsCommand = new Command(async () => await LoadToDoItems());
            AddToDoItemCommand = new Command<ToDoDto>(async (toDo) => await AddToDoItem(toDo));
            UpdateToDoItemCommand = new Command<ToDoDto>(async (toDo) => await UpdateToDoItem(toDo));
            DeleteToDoItemCommand = new Command<Guid>(async (id) => await DeleteToDoItem(id));
        }

        private async Task LoadToDoItems()
        {
            var response = await HttpClientHelper.GetAsync<List<ToDoDto>>(BaseUrl);

            if (response.IsSuccessStatusCode)
            {
                var toDoItems = await response.Content.ReadFromJsonAsync<List<ToDoDto>>();
                ToDoItems = new ObservableCollection<ToDoDto>(toDoItems);
            }
            else
            {
                Console.WriteLine("Failed to fetch ToDo items: " + response.ReasonPhrase);
            }
        }

        private async Task AddToDoItem(ToDoDto toDo)
        {
            var json = JsonConvert.SerializeObject(toDo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClientHelper.PostAsync(BaseUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Failed to add ToDo item: " + response.ReasonPhrase);
            }
            else
            {
                await LoadToDoItems(); // Reload ToDo items after addition
            }
        }

        private async Task UpdateToDoItem(ToDoDto toDo)
        {
            var url = $"{BaseUrl}/{toDo.ToDoId}";
            var json = JsonConvert.SerializeObject(toDo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClientHelper.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Failed to update ToDo item: " + response.ReasonPhrase);
            }
            else
            {
                await LoadToDoItems(); // Reload ToDo items after update
            }
        }

        private async Task DeleteToDoItem(Guid id)
        {
            var url = $"{BaseUrl}/{id}";
            var response = await HttpClientHelper.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Failed to delete ToDo item: " + response.ReasonPhrase);
            }
            else
            {
                var itemToRemove = ToDoItems.FirstOrDefault(t => t.ToDoId == id);
                if (itemToRemove != null)
                    ToDoItems.Remove(itemToRemove);

            }
        }
    }
}
