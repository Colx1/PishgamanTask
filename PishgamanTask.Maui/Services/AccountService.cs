using PishgamanTask.Maui.Dtos;
using PishgamanTask.Maui.Interface;
using System.Net.Http;
using System.Net.Http.Json;
using static PishgamanTask.Maui.Dtos.ServiceResponse;

namespace PishgamanTask.Maui.Services
{
    public class AccountService : IAccountService
    {
		private readonly HttpClient _httpClient;
        IHttpClientFactory httpClientFactory;

		public AccountService(HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClient;
			this.httpClientFactory = httpClientFactory;
		}

        public async Task<LoginResponse> LogInAccountAsync(LoginDto model)
        {
			//var response = await _httpClient.PostAsJsonAsync("api/Account/Login", model);
			//var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
			//return result!;

			var httpClient = httpClientFactory.CreateClient("custom-httpclient");
			var response = await httpClient.PostAsJsonAsync("api/Account/Login", model);
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
