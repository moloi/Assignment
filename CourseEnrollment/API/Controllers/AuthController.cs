using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CourseEnrollment.API.Data;
using CourseEnrollment.API.Services;
using CourseEnrollment.Shared.DTOs;

namespace CourseEnrollment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var token = _tokenService.GenerateToken(user.Id, user.Email!);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            UserId = user.Id
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized("Invalid credentials");

        var token = _tokenService.GenerateToken(user.Id, user.Email!);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            UserId = user.Id
        });
    }
}
