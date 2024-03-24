using FlowZone.shared.Dtos;
using Refit;
using System;
using System.Threading.Tasks;

namespace FlowZone.Services
{
    public interface IToDosApi
    {
        [Get("/api/ToDo")]
        Task<ResultWithDataDto<List<ToDoDto>>> GetAllToDoItems();

        [Get("/api/ToDo/{id}")]
        Task<ResultWithDataDto<ToDoDto>> GetToDoItem(Guid id);

        [Post("/api/ToDo")]
        Task<ResultWithDataDto<ToDoDto>> CreateToDoItem(ToDoDto toDoDto);

        [Put("/api/ToDo/{id}")]
        Task<ResultDto> UpdateToDoItem(Guid id, ToDoDto toDoDto);

        [Delete("/api/ToDo/{id}")]
        Task<ResultDto> DeleteToDoItem(Guid id);
    }
}
