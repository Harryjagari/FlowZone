using FlowZone.shared.Dtos;
using Refit;

namespace FlowZone.Services
{
    [Headers("Authorization: Bearer")]
    public interface IProfileApi
    {
        [Get("/api/Profile")]
        Task<ResultWithDataDto<List<ProfileDto>>> GetProfile();

        [Post("/api/Profile/set-profile-picture")]
        Task<ResultWithDataDto<string>> SetProfilePicture(Guid avatarId);
    }
}
