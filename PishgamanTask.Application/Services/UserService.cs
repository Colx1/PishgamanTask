using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PishgamanTask.Application.Interfaces;
using PishgamanTask.Domain.DtoContracts;
using PishgamanTask.Domain.Entities;

namespace PishgamanTask.Application.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAccountService
    {
        public async Task<ServiceResponse.ModelResponse> CreateAccount(AppUserDTO appUserDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse.LoginResponse> LoginAccount(LoginDTO loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
