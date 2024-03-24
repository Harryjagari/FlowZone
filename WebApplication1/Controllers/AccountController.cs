using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    // GET: Account/Login
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string username, string password)
    {
        // Validate username and password
        if (username == "admin" && password == "password")
        {
            // Redirect to dashboard upon successful login
            return RedirectToAction("index", "Home");
        }
        else
        {
            // Display an error message for invalid credentials
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }
    }
}
