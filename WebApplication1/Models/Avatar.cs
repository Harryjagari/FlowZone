using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Avatar
    {
        public Guid AvatarId { get; set; }

        public required string Name { get; set; }

        public double Price { get; set; }

        public required string ImagePath { get; set; }
    }
}