
using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Text;
using System.Text.Json;


namespace WebApplication1.Controllers
{
    public class AvatarController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AvatarController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7026/api/Avatar");

            if (response.IsSuccessStatusCode)
            {
                var avatars = await response.Content.ReadFromJsonAsync<IEnumerable<AvatarDto>>();

                foreach (var avatar in avatars)
                {
                    avatar.ImagePath = $"https://localhost:7026/api/Avatar/image/{avatar.ImagePath}";
                }

                return View(avatars);
            }
            else
            {
                return View("Error");
            }
        }


        public async Task<IActionResult> GetAvatar(Guid id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7026/api/Avatar/ById/{id}");

            if (response.IsSuccessStatusCode)
            {
                var UpdateAvatar = await response.Content.ReadFromJsonAsync<IEnumerable<AvatarUpdateRequestDto>>();
                return View(UpdateAvatar);
            }
            else
            {
                return View("Error");
            }
        }




        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AvatarRequestDto avatar)
        {
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(new StringContent(avatar.avatarName), "avatarName");
                        formData.Add(new StringContent(avatar.avatarPrice.ToString()), "avatarPrice");

                        using (var imageStream = avatar.avatarImage.OpenReadStream())
                        {
                            var imageContent = new StreamContent(imageStream);
                            imageContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                            {
                                Name = "\"avatarImage\"",
                                FileName = "\"" + avatar.avatarImage.FileName + "\""
                            };
                            formData.Add(imageContent);

                            var response = await client.PostAsync("https://localhost:7026/api/Avatar", formData);

                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                var errorMessage = await response.Content.ReadAsStringAsync();
                                Console.WriteLine($"Failed to create avatar. Error: {errorMessage}");
                                return BadRequest($"Failed to create avatar. Error: {errorMessage}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return BadRequest($"Failed to create avatar. Error: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<IActionResult> Update(Guid id, [FromForm]AvatarUpdateRequestDto avatarUpdateRequestDto)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.PutAsJsonAsync($"https://localhost:7026/api/Avatar/{id}", avatarUpdateRequestDto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7026/api/Avatar/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
