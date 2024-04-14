using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace FlowZone.Services
{
    [Headers("Authorization: Bearer")]
    public interface IToDosApi
    {
        [Get("/api/ToDo")]
        Task<ResultWithDataDto<List<ToDoDto>>> GetAllToDoItems();

        [Get("/api/ToDo/{id}")]
        Task<ResultWithDataDto<ToDoDto>> GetToDoItem(Guid id);

        [Get("/api/ToDo/Nearest")]
        Task<ResultWithDataDto<List<ToDoDto>>> GetNearestToDoItems();

        [Post("/api/ToDo")]
        Task<ResultWithDataDto<ToDoDto>> CreateToDoItem(CreateToDoDto toDoDto);

        [Put("/api/ToDo/{id}")]
        Task<ResultDto> UpdateToDoItem(Guid id, CreateToDoDto toDoDto);

        [Delete("/api/ToDo/{id}")]
        Task<ResultDto> DeleteToDoItem(Guid id);

        [Put("/api/ToDo/CompleteToDo/{ToDoId}")]
        Task<ResultWithDataDto<string>> CompleteToDo(Guid ToDoId);
    }
}
