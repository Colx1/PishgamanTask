using PishgamanTask.Maui.Dtos;
using PishgamanTask.Maui.Interface;
using System.Net.Http.Json;

namespace PishgamanTask.Maui.Services
{
    public class PersonService : IPersonService
    {
        private readonly HttpClient _httpClient;

        public PersonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            return await _httpClient.DeleteFromJsonAsync<bool>($"/api/Repository/DeletePerson/{id}");
        }

        public async Task<IEnumerable<PersonDto>> GetAllPersonsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<PersonDto>>("/api/Repository/GetAllPersons");
        }

        public async Task<PersonDto> GetPersonAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PersonDto>($"/api/Repository/GetPerson/{id}");
        }

        public async Task<PersonDto> InsertNewPersonAsync(PersonDto person)
        {
            var _person = await _httpClient.PostAsJsonAsync("/api/Repository/InsertPerson", person);
            var response = await _person.Content.ReadFromJsonAsync<PersonDto>();
            return response;
        }

        public async Task<PersonDto> UpdatePersonAsync(PersonDto person)
        {
            var _person = await _httpClient.PutAsJsonAsync("/api/Repository/UpdatePerson", person);
            var response = await _person.Content.ReadFromJsonAsync<PersonDto>();
            return response;
        }
    }
}
