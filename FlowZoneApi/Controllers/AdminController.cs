using FlowZone.shared.Dtos;
using FlowZoneApi.Data;
using FlowZoneApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;


namespace FlowZoneApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController(DataContext context) : ControllerBase
    {
        private readonly DataContext _context = context;

        [HttpPost("add")]
        public IActionResult AddAdmin([FromBody] AdminLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username is already taken
            if (_context.Admins.Any(a => a.AdminUserName == model.Username))
            {
                return Conflict(new { message = "Username is already taken" });
            }

            // Hash the password using a secure hashing algorithm (e.g., bcrypt)
            string hashedPassword = HashPassword(model.Password);

            // Create a new Admin entity
            var admin = new Admin
            {
                AdminId = Guid.NewGuid(),
                AdminUserName = model.Username,
                AdminPassword = hashedPassword
            };

            // Add the new admin to the database
            _context.Admins.Add(admin);
            _context.SaveChanges();

            return Ok(new { message = "Admin added successfully" });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Compute hash from password
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AdminLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find admin by username
            var admin = _context.Admins.FirstOrDefault(a => a.AdminUserName == model.Username);

            // Check if admin exists and password matches
            if (admin == null || !VerifyPassword(model.Password, admin.AdminPassword))
            {
                return BadRequest(new { message = "Invalid username or password" });
            }

            return Ok(new { message = "Login successful" });
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                // Compute hash from input password
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));

                // Convert byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                // Compare the hashed input password with the stored hashed password
                return builder.ToString() == hashedPassword;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin(Guid id)
        {
            // Find the admin by id
            var admin = _context.Admins.Find(id);

            if (admin == null)
            {
                return NotFound(new { message = "Admin not found" });
            }

            // Remove the admin from the context
            _context.Admins.Remove(admin);
            _context.SaveChanges();

            return Ok(new { message = "Admin deleted successfully" });
        }

        [HttpGet]
        public IActionResult GetAllAdmins()
        {
            var admins = _context.Admins.ToList();
            return Ok(admins);
        }



    }


}


