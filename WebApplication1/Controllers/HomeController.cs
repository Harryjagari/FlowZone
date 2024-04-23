using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _clientFactory.CreateClient();

                var userCountResponse = await client.GetAsync("https://localhost:7026/api/Dashboard/user-count");
                var userCount = await userCountResponse.Content.ReadFromJsonAsync<int>();

                var avatarCountResponse = await client.GetAsync("https://localhost:7026/api/Dashboard/avatar-count");
                var avatarCount = await avatarCountResponse.Content.ReadFromJsonAsync<int>();

                var challengeCountResponse = await client.GetAsync("https://localhost:7026/api/Dashboard/challenge-count");
                var challengeCount = await challengeCountResponse.Content.ReadFromJsonAsync<int>();


                var dashboardData = new DashboardViewModel
                {
                    UserCount = userCount,
                    AvatarCount = avatarCount,
                    ChallengeCount = challengeCount,
                };

                return View(dashboardData); 
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

    }
}
