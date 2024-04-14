using FlowZone.shared.Dtos;
using Refit;

namespace FlowZone.Services
{
    public interface IChallengeApi
    {
        [Get("/api/ChallengeMobile")]
        Task<ResultWithDataDto<List<ChallengeDto>>> GetChallenges();

        [Post("/api/UserChallenge/JoinChallenge/{ChallengeId}")]
        [Headers("Authorization: Bearer")]
        Task<ResultWithDataDto<string>> JoinChallenges(Guid ChallengeId);


        [Get("/api/UserChallenge/GetUncompletedChallenges")]
        [Headers("Authorization: Bearer")]
        Task<ResultWithDataDto<List<UserChallengeDto>>> GetUserChallenges();


        [Put("/api/UserChallenge/CompleteChallenge/{userChallengeId}")]
        [Headers("Authorization: Bearer")]
        Task<ResultWithDataDto<string>> CompleteChallenge(Guid userChallengeId);
    }
}
