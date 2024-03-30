using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class AvatarApiClient
    {
        private readonly HttpClient _httpClient;

        public AvatarApiClient(string baseUri)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<AvatarDto>> GetAvatarsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Avatar");
            response.EnsureSuccessStatusCode(); // Throw if not a success code
            return await response.Content.ReadAsAsync<IEnumerable<AvatarDto>>();
        }

        public async Task<Uri> CreateAvatarAsync([FromForm]AvatarRequestDto avatar)
        {
            using var formData = new MultipartFormDataContent();
            var avatarJson = JsonConvert.SerializeObject(avatar);
            formData.Add(new StringContent(avatarJson), "avatar");

            if (avatar.avatarImage != null)
            {
                var imageStream = new MemoryStream();
                await avatar.avatarImage.CopyToAsync(imageStream);
                imageStream.Seek(0, SeekOrigin.Begin);

                var imageContent = new StreamContent(imageStream);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(avatar.avatarImage.ContentType);

                formData.Add(imageContent, "avatarImage", avatar.avatarImage.FileName);
            }

            var response = await _httpClient.PostAsync("/api/Avatar", formData);
            response.EnsureSuccessStatusCode(); // Throw if not a success code

            return response.Headers.Location;
        }

        public async Task UpdateAvatarAsync(Guid id, AvatarDto avatar)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Avatar/{id}", avatar);
            response.EnsureSuccessStatusCode(); // Throw if not a success code
        }

        public async Task DeleteAvatarAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Avatar/{id}");
            response.EnsureSuccessStatusCode(); // Throw if not a success code
        }
    }
}
