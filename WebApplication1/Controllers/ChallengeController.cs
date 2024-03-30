using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class ChallengeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ChallengeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7026/api/Challenge");

            if (response.IsSuccessStatusCode)
            {
                var challenges = await response.Content.ReadFromJsonAsync<IEnumerable<ChallengeDto>>();
                return View(challenges);
            }
            else
            {
                // Handle error response
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChallengeDto challengeDto)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7026/api/Challenge", challengeDto);

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


        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [FromForm]ChallengeDto challengeDto)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.PutAsJsonAsync($"https://localhost:7026/api/Challenge/{id}", challengeDto);

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
            var response = await client.DeleteAsync($"https://localhost:7026/api/Challenge/{id}");

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
