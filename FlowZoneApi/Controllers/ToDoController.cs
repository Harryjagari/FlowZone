using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlowZoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all actions in this controller
    public class ToDoController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;

        // GET: api/ToDo
        [HttpGet]
        [Authorize]
        public async Task<ResultWithDataDto<List<ToDoDto>>> GetAllToDoItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var toDoItems = await _context.ToDos
                                            .Where(t => t.UserId.ToString() == userId) // Convert Guid to string for comparison
                                            .Select(t => new ToDoDto(
                                                t.ToDoId, // ToDoId
                                                t.Title,
                                                t.Description,
                                                t.CreatedAt, // Assuming CreatedAt is the creation date of ToDo
                                                t.DueDate,
                                                t.Priority
                                            ))
                                            .ToListAsync();
            return ResultWithDataDto<List<ToDoDto>>.Success(toDoItems);
        }



        // GET: api/ToDo/1
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ResultWithDataDto<ToDoDto>> GetToDoItem(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var toDoItem = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == id);
            if (toDoItem == null)
            {
                return ResultWithDataDto<ToDoDto>.Failure("ToDo item not found");
            }

            var toDoDto = new ToDoDto(
                toDoItem.ToDoId,
                toDoItem.Title,
                toDoItem.Description,
                toDoItem.CreatedAt,
                toDoItem.DueDate,
                toDoItem.Priority
            );

            return ResultWithDataDto<ToDoDto>.Success(toDoDto);
        }


        // POST: api/ToDo
        [HttpPost]
        public async Task<ResultWithDataDto<ToDoDto>> CreateToDoItem(ToDoDto toDoDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var toDoItem = new ToDo
            {
                ToDoId = Guid.NewGuid(),
                Title = toDoDto.Title,
                Description = toDoDto.Description,
                CreatedAt = DateTime.Now,
                DueDate = toDoDto.DueDate,
                Priority = toDoDto.Priority
            };

            _context.ToDos.Add(toDoItem);
            await _context.SaveChangesAsync();

            return ResultWithDataDto<ToDoDto>.Success(toDoDto);
        }

        // PUT: api/ToDo/1
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ResultDto> UpdateToDoItem(Guid id, ToDoDto toDoDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingItem = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == id);
            if (existingItem == null)
            {
                return ResultDto.Failure("ToDo item not found");
            }

            existingItem.Title = toDoDto.Title;
            existingItem.Description = toDoDto.Description;
            existingItem.DueDate = toDoDto.DueDate;
            existingItem.Priority = toDoDto.Priority;

            await _context.SaveChangesAsync();

            return ResultDto.Success();
        }

        // DELETE: api/ToDo/1
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ResultDto> DeleteToDoItem(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var toDoItem = await _context.ToDos.FirstOrDefaultAsync(t => t.ToDoId == id);
            if (toDoItem == null)
            {
                return ResultDto.Failure("ToDo item not found");
            }

            _context.ToDos.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return ResultDto.Success();
        }
    }
}
