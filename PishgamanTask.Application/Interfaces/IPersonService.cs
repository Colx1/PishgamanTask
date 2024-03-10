using PishgamanTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishgamanTask.Application.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPersonsAsync();

        Task<Person> GetPersonAsync(int id);

        Task<Person> InsertNewPersonAsync(Person person);

        Task<Person> UpdatePersonAsync(Person person);

        Task<bool> DeletePersonAsync(int id);
    }
}
