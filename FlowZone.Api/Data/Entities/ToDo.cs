using System.ComponentModel.DataAnnotations;

namespace FlowZone.Api.Data.Entities
{
	public class ToDo
	{
		[Key]
		public Guid ToDoId { get; set; }

		[Required, MaxLength(180)]
		public string Title { get; set; }

		[Required, MaxLength(255)]
		public string Description { get; set; }

		[Required]
		public DateTime Created { get; set; }

		[Required]
		public DateTime LastUpdated { get; set; }

		[Required]
		public DateTime DueDate { get; set; }

		[Required, MaxLength(100)]
		public string Priority { get; set; }

	}
}
