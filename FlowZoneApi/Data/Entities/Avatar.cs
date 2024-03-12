using System.ComponentModel.DataAnnotations;

namespace FlowZoneApi.Data.Entities
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

        // Navigation property
        public List<UserAvatar> UserAvatars { get; set; }
    }
}
