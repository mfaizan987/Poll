//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Poll.Application.Dtos.Auth;
//using Poll.Domain.Entity;
//using Poll.Infrastructure.Data;

//[ApiController]
//[Route("api/[controller]")]
//public class AccountController : ControllerBase
//{
//    private readonly AppDbContext _context;

//    public AccountController(AppDbContext context)
//    {
//        _context = context;
//    }

//    [HttpPost("register")]
//    public async Task<IActionResult> Register(RegisterDto dto)
//    {
//        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
//            return BadRequest("Email already exists");

//        var user = new UserEntity
//        {
//            UserName = dto.UserName,
//            Email = dto.Email,
//            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
//        };

//        await _context.Users.AddAsync(user);
//        await _context.SaveChangesAsync();

//        //return Ok("User registered successfully");
//    }

//    [HttpPost("login")]
//    public async Task<IActionResult> Login(LoginDto dto)
//    {
//        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
//            return Unauthorized("Invalid credentials");

//        var roles = new List<string>();
//        return Ok(new { roles });
//    }
//}
