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
        public string avatarName { get; set; } // Adjusted property name to match form input name
        public double avatarPrice { get; set; } // Adjusted property name to match form input name
        public IFormFile avatarImage { get; set; }
    }
}
