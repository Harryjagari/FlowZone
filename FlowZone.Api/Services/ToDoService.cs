using FlowZone.Api.Data;
using FlowZone.shared.Dtos;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace FlowZone.Api.Services
{
	
	public class ToDoService(DataContext context)
	{
		private readonly DataContext _context = context;
		public async Task<ToDoDto[]> GetToDosAsync() =>
			await _context.ToDos.AsNoTracking()
			.Select(i => 
					new ToDoDto(
						i.ToDoId,
						i.Title,
						i.Description,
						i.Created,
						i.LastUpdated,
						i.DueDate,
						i.Priority)
			).ToArrayAsync();
	}
}
