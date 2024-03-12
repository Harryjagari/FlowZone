using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlowZoneApi.Data.Entities
{
    public class UserChallenge
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Challenge")]
        public Guid ChallengeId { get; set; }
        public Challenge Challenge { get; set; }

        // Define a composite primary key using UserId and ChallengeId
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserChallengeId { get; set; }

        public bool CompletionStatus { get; set; }
    }
}
