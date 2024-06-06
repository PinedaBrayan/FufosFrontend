
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;

namespace FufosFrontend.Services;

public class UserAuthenticationService
    (ApiService apiService, ILocalStorageService localStorageService, IHttpContextAccessor ContextAccessor)
{
    HttpContext HttpContext => ContextAccessor.HttpContext!;
    public async Task<bool> Login(string Email, string Password)
    {
        var Url = apiService.SearchAuthenticationEndpoint();
        using var client = new HttpClient();

        var User = new
        {
            Email, Password
        };

        var ValueContent = JsonConvert.SerializeObject(User);

        var Content = new StringContent(ValueContent, Encoding.UTF8, "application/json");

        var Request = await client.PostAsync($"{Url}api/Login/", Content)
            .ConfigureAwait(true);

        await CreateToken(Request);

        return Request.IsSuccessStatusCode;
    }

    public async Task<bool> Register(string Email, string Password)
    {
        var Url = apiService.SearchAuthenticationEndpoint();
        using var client = new HttpClient();

        var User = new
        {
            Email,
            Password
        };

        var ValueContent = JsonConvert.SerializeObject(User);

        var Content = new StringContent(ValueContent, Encoding.UTF8, "application/json");

        var Request = await client.PostAsync($"{Url}api/Register/", Content)
            .ConfigureAwait(true);

        await CreateToken(Request);

        return Request.IsSuccessStatusCode;
    }

    private async Task CreateToken(HttpResponseMessage Request)
    {
        try
        {
            string responseContent = await Request.Content.ReadAsStringAsync();
            var Response = (JObject) JsonConvert.DeserializeObject(responseContent)!;
            var Token = Response.GetValue("data")!.ToString();

            await localStorageService.SetItemAsync("usertoken", Token);

            var claims = new[] {
                new Claim(ClaimTypes.Name, "NOMBRE QUEMADO"),
                new Claim("AccessToken", Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Bearer");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
        catch
        {
            //Do nothing
        }
    }

    public async Task Logout(HttpContext HttpContext)
    {
        try
        {
            await localStorageService.RemoveItemAsync("usertoken");
            await HttpContext.SignOutAsync();
        }
        catch
        {
            //Do nothing
        }
    }
}