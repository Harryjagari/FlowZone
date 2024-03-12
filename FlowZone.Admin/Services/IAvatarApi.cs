using FlowZone.shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.Admin.Services
{
    public interface IAvatarApi
    {
        [Post("/api/Avatars")]
        Task<Guid> SaveAvatarAsync(AvatarDto avatarDto);
    }
}