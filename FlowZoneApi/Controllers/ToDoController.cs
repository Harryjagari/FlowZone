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
        // GET: api/ToDo
        [HttpGet]
        public async Task<ResultWithDataDto<List<ToDoDto>>> GetAllToDoItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDate = DateTime.Now;

            var toDoItems = await _context.ToDos
                .Where(t => t.UserId.ToString() == userId && !t.IsComplete && t.DueDate > currentDate)
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



        [HttpGet("Nearest")]
        public async Task<ResultWithDataDto<List<ToDoDto>>> GetNearestToDoItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDate = DateTime.Now;

            var nearestToDos = await _context.ToDos
                .Where(t => t.UserId.ToString() == userId && !t.IsComplete && t.DueDate > currentDate)
                .OrderBy(t => t.DueDate)
                .Take(3) 
                .Select(t => new ToDoDto(
                    t.ToDoId,
                    t.Title,
                    t.Description,
                    t.CreatedAt,
                    t.DueDate,
                    t.Priority
                ))
                .ToListAsync();

            return ResultWithDataDto<List<ToDoDto>>.Success(nearestToDos);
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
        public async Task<ResultWithDataDto<ToDoDto>> CreateToDoItem(CreateToDoDto toDoDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(Guid.Parse(userId));

            if (user == null)
            {
                // Handle the scenario where the user is not found
                return ResultWithDataDto<ToDoDto>.Failure("User not found");
            }

            var toDoItem = new ToDo
            {
                ToDoId = Guid.NewGuid(),
                Title = toDoDto.Title,
                Description = toDoDto.Description,
                CreatedAt = DateTime.Now,
                DueDate = toDoDto.DueDate,
                Priority = toDoDto.Priority,
                IsComplete = false, // Defaulting to false as the todo is just created
                UserId = user.UserId // Set the UserId for the ToDo item
            };

            // Add the ToDo item to the context
            _context.ToDos.Add(toDoItem);

            await _context.SaveChangesAsync();

            // Return the created ToDo item
            var createdToDoDto = new ToDoDto(
                toDoItem.ToDoId,
                toDoItem.Title,
                toDoItem.Description,
                toDoItem.CreatedAt,
                toDoItem.DueDate,
                toDoItem.Priority
            );

            return ResultWithDataDto<ToDoDto>.Success(createdToDoDto);
        }



        // PUT: api/ToDo/1
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ResultDto> UpdateToDoItem(Guid id, CreateToDoDto toDoDto)
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




        [HttpPut("CompleteToDo/{ToDoId}")]
        public async Task<ResultWithDataDto<string>> CompleteToDo(Guid ToDoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userToDo = await _context.ToDos.FindAsync(ToDoId);

            if (userToDo == null)
            {
                return new ResultWithDataDto<string>(false, null, "User ToDo not found.");
            }

            if (userToDo.UserId != Guid.Parse(userId))
            {
                return new ResultWithDataDto<string>(false, null, "User is not authorized to complete this ToDo.");
            }

            if (userToDo.IsComplete)
            {
                return new ResultWithDataDto<string>(false, null, "ToDo is already completed.");
            }

            // Update the challenge as completed
            userToDo.IsComplete = true;
            _context.ToDos.Update(userToDo);

            await _context.SaveChangesAsync();

            return new ResultWithDataDto<string>(true, "ToDo completed successfully", null);
        }
    }
}
