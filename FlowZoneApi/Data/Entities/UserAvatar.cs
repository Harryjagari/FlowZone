using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlowZoneApi.Data.Entities
{
    public class UserAvatar
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Avatar")]
        public Guid AvatarId { get; set; }
        public Avatar Avatar { get; set; }

        public DateTime PurchaseDate { get; set; }

        [Key] // Define a composite primary key using UserId and AvatarId
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure that the primary key is auto-generated
        public Guid UserAvatarId { get; set; }
    }
}
