using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class AccountController : Controller
{

    private readonly IHttpClientFactory _clientFactory;

    public AccountController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public ActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromForm] AdminLoginDto adminLoginDto)
    {
        if (string.IsNullOrEmpty(adminLoginDto.Username) || string.IsNullOrEmpty(adminLoginDto.Password))
        {
            ModelState.AddModelError(string.Empty, "Username and password are required.");
            return BadRequest(ModelState);
        }

        var client = _clientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"https://localhost:7026/api/Admin/Login", adminLoginDto);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("index", "Home");
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return BadRequest(ModelState);
        }
        else
        {

            return View("Error");
        }
    }

    public IActionResult Logout()
    {

        return RedirectToAction("Login");
    }

}
