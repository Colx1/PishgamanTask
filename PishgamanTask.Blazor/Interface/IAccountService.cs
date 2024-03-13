using PishgamanTask.Blazor.Dtos;
using static PishgamanTask.Blazor.Dtos.ServiceResponse;

namespace PishgamanTask.Blazor.Interface
{
    public interface IAccountService
    {
        Task<LoginResponse> LogInAccountAsync(LoginDto model);

        Task<RegisterResponse> RegisterAccountAsync(RegisterDto model);
    }
}
