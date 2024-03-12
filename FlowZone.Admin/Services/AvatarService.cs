using System;
using System.Net.Http;
using System.Threading.Tasks;
using FlowZone.shared.Dtos;
using System.Net.Http.Json;

namespace FlowZone.Admin.Services
{
    public class AvatarService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<Guid> SaveAvatarAsync(AvatarDto avatarDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/avatars", avatarDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task DeleteAvatarAsync(Guid avatarId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/avatars/{avatarId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
