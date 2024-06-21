using PishgamanTask.Maui.Dtos;
using static PishgamanTask.Maui.Dtos.ServiceResponse;

namespace PishgamanTask.Maui.Interface
{
    public interface IAccountService
    {
        Task<LoginResponse> LogInAccountAsync(LoginDto model);

        Task<RegisterResponse> RegisterAccountAsync(RegisterDto model);
    }
}
