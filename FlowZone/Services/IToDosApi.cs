using FlowZone.shared.Dtos;
using Refit;

namespace FlowZone.Services
{
	public interface IToDosApi
	{
		[Get("/api/ToDos")]
		Task<ToDoDto[]> GetToDosAsync();
	}
}
