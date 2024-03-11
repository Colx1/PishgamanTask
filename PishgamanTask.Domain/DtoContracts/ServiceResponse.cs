using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishgamanTask.Domain.DtoContracts
{
    public class ServiceResponse
    {
        public record class ModelResponse(bool flag, string Message);
        public record class LoginResponse(bool flag, string token, string Message);
    }
}
