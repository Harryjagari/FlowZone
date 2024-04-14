using FlowZone.shared.Dtos;
using Refit;

namespace FlowZone.Services
{
    public interface IAvatarApi
    {
        [Get("/api/AvatarMobile")]
        Task<ResultWithDataDto<List<AvatarDto>>> GetAvatars();

        
        [Post("/api/AvatarPurchase/{avatarId}")]
        [Headers("Authorization: Bearer")]
        Task<ResultWithDataDto<string>> PurchaseAvatar(Guid avatarId);


        [Get("/api/AvatarPurchase")]
        [Headers("Authorization: Bearer")]
        Task<ResultWithDataDto<List<UserAvatarDto>>> GetPurchasedAvatars();
    }
}
