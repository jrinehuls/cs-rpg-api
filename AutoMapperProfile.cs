using AutoMapper;
using RPG_API.Dtos;

namespace RPG_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}
