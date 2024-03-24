using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvatarController : ControllerBase
    {
        Uri baseAddress = new Uri("https://localhost:44342/api");
        private readonly HttpClient _httpClient;

        public AvatarController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        // Method to get avatars from API
        public async Task<IEnumerable<AvatarDto>> GetAvatarsAsync()
        {
            var response = await _httpClient.GetAsync("api/Avatar");
            response.EnsureSuccessStatusCode(); // Throw if not success
            return await response.Content.ReadFromJsonAsync<IEnumerable<AvatarDto>>();
        }

        // Method to add avatar via API
        public async Task<AvatarDto> AddAvatarAsync(AvatarDto avatar)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Avatar", avatar);
            response.EnsureSuccessStatusCode(); // Throw if not success
            return await response.Content.ReadFromJsonAsync<AvatarDto>();
        }

        // Method to update avatar via API
        public async Task UpdateAvatarAsync(Guid id, AvatarDto avatar)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Avatar/{id}", avatar);
            response.EnsureSuccessStatusCode(); // Throw if not success
        }

        // Method to delete avatar via API
        public async Task DeleteAvatarAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Avatar/{id}");
            response.EnsureSuccessStatusCode(); // Throw if not success
        }
    }
}
