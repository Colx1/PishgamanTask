using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PishgamanTask.Maui.GenericPrincipal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PishgamanTask.Maui.Authentication
{
    public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private const string LocalStorageKey = "auth";
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await localStorageService.GetItemAsStringAsync(LocalStorageKey)!;
            if (string.IsNullOrEmpty(token))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var (name, email) = GetClaims(token);
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claims = SetClaimPrincipal(name, email);
            if (claims is null)
                return await Task.FromResult(new AuthenticationState(anonymous));
            else
                return await Task.FromResult(new AuthenticationState(claims));
        }

        public async Task UpdateAuthenticationState(string jwtToken)
        {
            var claims = new ClaimsPrincipal();
            if(!string.IsNullOrEmpty(jwtToken))
            {
                var (name, email) = GetClaims(jwtToken);
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
                    return;

                var setClaims = SetClaimPrincipal(name, email);
                if (setClaims is null)
                    return;

                await localStorageService.SetItemAsStringAsync(LocalStorageKey, jwtToken);
            }
            else
            {
                await localStorageService.RemoveItemAsync(LocalStorageKey);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
        }

        public static ClaimsPrincipal SetClaimPrincipal(string name, string email)
        {
            if (name == null || email == null)
                return new ClaimsPrincipal();

            return new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new (ClaimTypes.Name, name!),
                    new (ClaimTypes.Email, email!)
                ], "JwtAuth"));
        }

        private static (string, string) GetClaims(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return (null!, null!);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var name = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;
            var email = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            return (name, email);
        }
    }
}
