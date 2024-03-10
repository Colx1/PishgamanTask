using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PishgamanTask.Application.Interfaces;
using PishgamanTask.Domain.Entities;
using PishgamanTask.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PishgamanTask.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PishgamanContext _pishgamanDbContext;
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(PishgamanContext pishgamanDbContext, ILogger<PersonRepository> logger)
        {
            _pishgamanDbContext = pishgamanDbContext;
            _logger = logger;

            _logger.LogInformation($"AlbumRepository ctor");
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            var person = await _pishgamanDbContext.Tbl_People.FirstOrDefaultAsync(x => x.Id == id);
            if (person != null)
            {
                _pishgamanDbContext.Remove(person);
                await _pishgamanDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            _logger.LogInformation($"GetAllPersonsAsync");

            var people = await _pishgamanDbContext.Tbl_People.ToListAsync();
            return people;
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            _logger.LogInformation($"GetPersonAsync - ID: {id}");

            var person = await _pishgamanDbContext.Tbl_People.FirstOrDefaultAsync(x => x.Id == id);
            return person;
        }

        public async Task<Person> InsertNewPersonAsync(Person person)
        {
            _logger.LogInformation($"InsertNewPersonAsync - {JsonSerializer.Serialize(person)}");

            _pishgamanDbContext.Tbl_People.Add(person);
            await _pishgamanDbContext.SaveChangesAsync();

            return person;
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            _logger.LogInformation($"UpdatePersonAsync - {JsonSerializer.Serialize(person)}");

            _pishgamanDbContext.Tbl_People.Update(person);
            await _pishgamanDbContext.SaveChangesAsync();

            return person;
        }
    }
}
