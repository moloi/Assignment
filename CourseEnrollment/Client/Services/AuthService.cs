using System.Net.Http.Json;
using CourseEnrollment.Shared.DTOs;

namespace CourseEnrollment.Client.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> Register(RegisterDto dto);
    Task<AuthResponseDto?> Login(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<AuthResponseDto?> Register(RegisterDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", dto);
        
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        return null;
    }

    public async Task<AuthResponseDto?> Login(LoginDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", dto);
        
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<AuthResponseDto>();

        return null;
    }
}
