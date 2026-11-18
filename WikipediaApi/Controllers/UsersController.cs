using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikipediaApi.Data;
using WikipediaApi.Models;

namespace WikipediaApi.Controllers;

public record UserDto(
    int UserId,
    string Login,
    string Email,
    int? RoleId,
    DateTime CreatedAt);

// PasswordHash хранится как есть (без хеширования).
// Столбец password_hash в БД — максимум 20 символов.
public record CreateUserDto(
    string Login,
    string PasswordHash,
    string Email,
    int? RoleId);

public record UpdateUserDto(
    string Login,
    string Email,
    int? RoleId,
    string? Password = null);

public record LoginDto(string Login, string Password);

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly WikipediaContext _db;

    public UsersController(WikipediaContext db)
    {
        _db = db;
    }

    private static UserDto ToDto(User u) =>
        new(u.UserId, u.Login, u.Email, u.RoleId, u.CreatedAt);

    // Проверка длин под ограничения столбцов БД
    private static string? Validate(string? login, string? email, string? password)
    {
        if (string.IsNullOrWhiteSpace(login) || login.Length > 30)
            return "Логин должен быть от 1 до 30 символов";

        if (string.IsNullOrWhiteSpace(email) || email.Length > 30 || !email.Contains('@'))
            return "Введите корректный email (не длиннее 30 символов)";

        if (password != null && (password.Length < 1 || password.Length > 20))
            return "Пароль должен быть от 1 до 20 символов";

        return null;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users.AsNoTracking().ToListAsync();
        return Ok(users.Select(ToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _db.Users.FindAsync(id);
        return user == null ? NotFound() : Ok(ToDto(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == dto.Login);

        // Пароль сравниваем в C# (строго, с учётом регистра),
        // т.к. collation БД регистронезависимый.
        if (user == null || user.PasswordHash != dto.Password)
            return Unauthorized(new { message = "Неверный логин или пароль" });

        return Ok(ToDto(user));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var error = Validate(dto.Login, dto.Email, dto.PasswordHash);
        if (error != null)
            return BadRequest(new { message = error });

        if (await _db.Users.AnyAsync(u => u.Login == dto.Login))
            return Conflict(new { message = "Этот логин уже занят" });

        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict(new { message = "Этот email уже зарегистрирован" });

        var user = new User
        {
            Login = dto.Login,
            PasswordHash = dto.PasswordHash,
            Email = dto.Email,
            RoleId = dto.RoleId,
            CreatedAt = DateTime.Now
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Get), new { id = user.UserId }, ToDto(user));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        var error = Validate(dto.Login, dto.Email, dto.Password);
        if (error != null)
            return BadRequest(new { message = error });

        if (await _db.Users.AnyAsync(u => u.Login == dto.Login && u.UserId != id))
            return Conflict(new { message = "Этот логин уже занят" });

        if (await _db.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != id))
            return Conflict(new { message = "Этот email уже зарегистрирован" });

        user.Login = dto.Login;
        user.Email = dto.Email;
        user.RoleId = dto.RoleId;

        if (!string.IsNullOrEmpty(dto.Password))
            user.PasswordHash = dto.Password;

        await _db.SaveChangesAsync();
        return Ok(ToDto(user));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
