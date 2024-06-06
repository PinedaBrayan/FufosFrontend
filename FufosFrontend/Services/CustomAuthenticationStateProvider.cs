using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blazored.LocalStorage;
using Fufos.Globals.Enums;
using FufosEntities.DTOS;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FufosFrontend.Services;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService, IConfiguration configuration) : AuthenticationStateProvider
{
    readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Se trae del localstorage aquel item cuya llave es "usertoken"
            var token = await localStorageService.GetItemAsync<string>("usertoken");

            if(string.IsNullOrEmpty(token))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var User = GetUserFromToken(token);

            var Role = string.Empty;

            if(User is {IsAdmin: true, IsEmployee: true} or {IsAdmin: true})
            {
                Role = $"{EnumRoles.Admin}";
            }else if(User is {IsEmployee: true})
            {
                Role = $"{EnumRoles.Employee}";
            }

            // Prepara la informacion de la sesion
            var claims = new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new(ClaimTypes.NameIdentifier, $"{User.Rowid}"),
                    new(ClaimTypes.Name, User.FullName),
                    new(ClaimTypes.Email, User.FullName),
                    new(ClaimTypes.Role, Role)
                ]
            , "JWTAuth"));

            // Retorna la sesion con el usuario ya autenticado
            return await Task.FromResult(new AuthenticationState(claims));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(anonymous));
        }
    }

    private JWTUserDTO GetUserFromToken(string Token)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(configuration["Authentication:SecretKey"]!);

        handler.ValidateToken(Token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var user = jwtToken.Claims.First(x => x.Type == "user");

        var Result = JsonConvert.DeserializeObject<JWTUserDTO>(user.Value!)!;

        return Result;
    }
}