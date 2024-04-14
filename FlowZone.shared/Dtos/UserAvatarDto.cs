﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.shared.Dtos
{
    public class UserAvatarDto
    {
        public Guid AvatarId { get; set; }
        public string avatarName { get; set; } // Adjusted property name to match form input name
        public int avatarPrice { get; set; } // Adjusted property name to match form input name
        public IFormFile avatarImage { get; set; }
        public string? ImagePath { get; set; }
    }
}
