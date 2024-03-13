using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PishgamanTask.Application.Interfaces;
using PishgamanTask.Domain.DtoContracts;
using PishgamanTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static PishgamanTask.Domain.DtoContracts.ServiceResponse;

namespace PishgamanTask.Infrastructure.Repositories
{
    public class AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IUserAccountService
    {
        public async Task<RegisterResponse> CreateAccount(AppUserDTO appUserDto)
        {
            if (appUserDto == null)
                return new RegisterResponse(false, "Model is empty!");

            var user = await userManager.FindByNameAsync(appUserDto.UserName);
            if(user != null)
                return new RegisterResponse(false, "User already exist in system!");

            var newUser = new ApplicationUser()
            {
                UserName = appUserDto.UserName,
                Email = appUserDto?.Email,
            };

            var createUser = await userManager.CreateAsync(newUser, appUserDto!.Password);
            if(!createUser.Succeeded)
                return new RegisterResponse(false, "Error occured.. " + createUser.Errors.FirstOrDefault()?.Description.ToString());


            //Probably gonna check this on program Startup not here. Would change it later
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new RegisterResponse(true, "Account Created");
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await userManager.AddToRoleAsync(newUser, "User");
                return new RegisterResponse(true, "Account Created");
            }

        }

        public async Task<LoginResponse> LoginAccount(LoginDTO loginDto)
        {
            if (loginDto == null)
                return new LoginResponse(false, null!, "Login container is empty");

            var getUser = await userManager.FindByNameAsync(loginDto.UserName);
            if (getUser is null)
                return new LoginResponse(false, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDto.Password);
            if (!checkUserPasswords)
                return new LoginResponse(false, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.UserName, getUser.Email, getUserRole.FirstOrDefault());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(int.Parse(config["Jwt:ExpiresInMinute"]!)),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
