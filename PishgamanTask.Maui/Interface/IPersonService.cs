using PishgamanTask.Maui.Dtos;

namespace PishgamanTask.Maui.Interface
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
