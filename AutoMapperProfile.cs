using AutoMapper;
using RPG_API.Dtos.Character;

namespace RPG_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            // <From, To>
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
        }
    }
}
