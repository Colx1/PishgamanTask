﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PishgamanTask.Application.Interfaces;
using PishgamanTask.Domain.DtoContracts;

namespace PishgamanTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccountService userAccount) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(AppUserDTO userDTO)
        {
            var response = await userAccount.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await userAccount.LoginAccount(loginDTO);
            return Ok(response);
        }
    }
}
