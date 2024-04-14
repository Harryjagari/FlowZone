using System.ComponentModel.DataAnnotations;

namespace FlowZoneApi.Data.Entities
{
	public class ToDo
	{
		[Key]
		public Guid ToDoId { get; set; }

		[Required, MaxLength(180)]
		public string Title { get; set; }

		[Required, MaxLength(255)]
		public string? Description { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public DateTime DueDate { get; set; }

		[Required, MaxLength(100)]
		public string Priority { get; set; }

        public bool IsComplete { get; set; }

        // Foreign key
        public Guid UserId { get; set; }
        // Navigation property
        public User User { get; set; }

        public List<UserToDo> UserTodos { get; set; }

    }
}
