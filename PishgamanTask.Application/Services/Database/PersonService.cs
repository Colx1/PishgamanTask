using PishgamanTask.Application.Interfaces;
using PishgamanTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishgamanTask.Application.Services.Database
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            return await _personRepository.DeletePersonAsync(id);
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await _personRepository.GetAllPersonsAsync();
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            return await _personRepository.GetPersonAsync(id);
        }

        public async Task<Person> InsertNewPersonAsync(Person person)
        {
            return await _personRepository.InsertNewPersonAsync(person);
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            return await _personRepository.UpdatePersonAsync(person);
        }
    }
}
