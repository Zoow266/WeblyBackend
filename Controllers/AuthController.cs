using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Хэшируем пароль перед сохранением
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok("Login successful.");
    }

    [HttpGet("check")]
    public IActionResult CheckDatabase()
    {
        // Проверяем, есть ли данные в базе данных
        var usersExist = _context.Users.Any();

        if (usersExist)
        {
            // Если данные есть, возвращаем их
            var users = _context.Users.ToList();
            return Ok(users);
        }
        else
        {
            // Если данных нет, возвращаем сообщение "все хорошо"
            return Ok("Все хорошо, но данных в базе нет.");
        }
    }
}