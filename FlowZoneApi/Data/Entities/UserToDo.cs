using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowZoneApi.Data.Entities
{
    public class UserToDo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserToDoId { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }


        [ForeignKey("ToDo")]
        public Guid ToDoId { get; set; }

        // Navigation properties

        public ToDo ToDo { get; set; }
    }
}
