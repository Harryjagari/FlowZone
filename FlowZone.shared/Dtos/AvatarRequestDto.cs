using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.shared.Dtos
{
    public class AvatarRequestDto
    {
        public string avatarName { get; set; } 
        public int avatarPrice { get; set; } 
        public IFormFile avatarImage { get; set; }
    }
}
