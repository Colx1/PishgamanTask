using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishgamanTask.Domain.DtoContracts
{
    public record UserSession(string? Id, string? UserName, string? Email, string? Role);
}
