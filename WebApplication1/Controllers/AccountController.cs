using FlowZone.shared.Dtos;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{

    private readonly IHttpClientFactory _clientFactory;

    public AccountController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    // GET: Account/Login
    public ActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromForm] AdminLoginDto adminLoginDto)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"https://localhost:7026/api/Admin/Login", adminLoginDto);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("index", "Home");
        }
        else
        {
            // Handle error response
            return View("Error");
        }
    }
}
