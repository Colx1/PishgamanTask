using PishgamanTask.Blazor.Dtos;
using PishgamanTask.Blazor.Interface;
using System.Net.Http.Json;
using static PishgamanTask.Blazor.Dtos.ServiceResponse;

namespace PishgamanTask.Blazor.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LogInAccountAsync(LoginDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/Login", model);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result!;
        }

        public async Task<RegisterResponse> RegisterAccountAsync(RegisterDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/Register", model);
            var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
            return result!;
        }
    }
}
