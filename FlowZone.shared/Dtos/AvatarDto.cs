using Microsoft.AspNetCore.Http;

namespace FlowZone.shared.Dtos
{
    public class AvatarDto
    {
        public Guid AvatarId { get; set; }
        public string avatarName { get; set; } // Adjusted property name to match form input name
        public int avatarPrice { get; set; } // Adjusted property name to match form input name
        public IFormFile avatarImage { get; set; }
        public string? ImagePath { get; set; }
    }
}
