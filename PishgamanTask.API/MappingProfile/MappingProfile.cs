using AutoMapper;
using PishgamanTask.API.DTOs;
using PishgamanTask.Domain.Entities;

namespace PishgamanTask.API.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDTO>().ReverseMap();
        }
    }
}
