
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

                // Update the image paths to use the new endpoint
                foreach (var avatar in avatars)
                {
                    avatar.ImagePath = $"https://localhost:7026/api/Avatar/image/{avatar.ImagePath}";
                }

                return View(avatars);
            }
            else
            {
                // Handle error response
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
                // Handle error response
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
                        // Add avatar data as form fields
                        formData.Add(new StringContent(avatar.avatarName), "avatarName");
                        formData.Add(new StringContent(avatar.avatarPrice.ToString()), "avatarPrice");

                        // Add image file
                        using (var imageStream = avatar.avatarImage.OpenReadStream())
                        {
                            var imageContent = new StreamContent(imageStream);
                            imageContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                            {
                                Name = "\"avatarImage\"",
                                FileName = "\"" + avatar.avatarImage.FileName + "\""
                            };
                            formData.Add(imageContent);

                            // Send the request to the API endpoint
                            var response = await client.PostAsync("https://localhost:7026/api/Avatar", formData);

                            // Check response status and handle accordingly
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                // Get the error message from the response content
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
                // Handle error response
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
                // Handle error response
                return View("Error");
            }
        }
    }
}
