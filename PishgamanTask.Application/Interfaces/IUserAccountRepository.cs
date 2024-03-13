﻿using PishgamanTask.Domain.DtoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PishgamanTask.Domain.DtoContracts.ServiceResponse;

namespace PishgamanTask.Application.Interfaces
{
    internal interface IUserAccountRepository
    {
        Task<RegisterResponse> CreateAccount(AppUserDTO appUserDto);

        Task<LoginResponse> LoginAccount(LoginDTO loginDto);
    }
}
