using Microsoft.AspNetCore.Http;

namespace FlowZone.shared.Dtos
{
    public class AvatarDto
    {
        public Guid AvatarId { get; set; }
        public string avatarName { get; set; } 
        public int avatarPrice { get; set; } 
        public IFormFile avatarImage { get; set; }
        public string? ImagePath { get; set; }
    }
}
