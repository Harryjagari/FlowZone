using System.ComponentModel.DataAnnotations;

namespace FlowZone.Api.Data.Entities
{
	public class Avatar
	{
		[Key]
		public Guid AvatarId { get; set; }

		[Required, MaxLength(50)]
		public string Name { get; set; }

		[Range(0.1,double.MaxValue)]
		public double Price { get; set; }

		[Required,MaxLength(180)]
		public string ImagePath { get; set; }

		public bool IsPurchased { get; set; }

		// Other properties related to avatars, such as description, etc.
	}
}
