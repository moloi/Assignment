using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace CourseEnrollment.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthStateProvider(HttpClient http, IJSRuntime jsRuntime)
    {
        _http = http;
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = null;
        try
        {
            token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }
        catch { }

        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task NotifyUserAuthentication(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        
        var claims = ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task NotifyUserLogout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        
        _http.DefaultRequestHeaders.Authorization = null;
        
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
