
using System.ComponentModel.DataAnnotations;

namespace FlowZoneApi.Data.Entities
{
	public class User
	{
		[Key]
		public Guid UserId { get; set; }

		[Required,MaxLength(30)]
		public string UserName { get; set; }

		[Required,MaxLength(100)]
		public string Email { get; set; }

		[Required,MaxLength(150)]
		public string Address { get; set; }

		[Required, MaxLength(20)]
		public string Salt { get; set; }

		[Required,MaxLength(180)]
		public string Hash { get; set; }

		[Required]
		public int CompletedChallenges { get; set; }

		[Required]
		public int EarnedPoints { get; set; }

        public string ? ResetPasswordOTP { get; set; }
        public DateTime? ResetPasswordOTPIssueTime { get; set; }

        public string? ProfilePictureUrl { get; set; }

        // Navigation property
        public List<UserToDo> UserTodos { get; set; }
        public List<Challenge> UserChallenges { get; set; }
        public List<Avatar> UserAvatars { get; set; }


    }

}
