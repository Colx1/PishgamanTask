using PishgamanTask.Blazor.Dtos;

namespace PishgamanTask.Blazor.Interface
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDto>> GetAllPersonsAsync();

        Task<PersonDto> GetPersonAsync(int id);

        Task<PersonDto> InsertNewPersonAsync(PersonDto person);

        Task<PersonDto> UpdatePersonAsync(PersonDto person);

        Task<bool> DeletePersonAsync(int id);
    }
}
