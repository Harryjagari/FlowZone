using System.ComponentModel.DataAnnotations;

namespace FlowZone.Api.Data.Entities
{
	public class Distraction
	{
		[Key]
		public Guid DistractionId { get; set; }

		[Required, MaxLength(30)]
		public string PackageName { get; set; }

		[Required, MaxLength(100)]
		public string TimeSpan { get; set; }

	}
}
