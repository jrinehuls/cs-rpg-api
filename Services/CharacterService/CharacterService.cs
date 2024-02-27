
using AutoMapper;
using RPG_API.Dtos;

namespace RPG_API.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private List<Character> characters = new List<Character> {
            new Character(),
            new Character { Id = 1, Name = "Sam"}
        };

        private IMapper mapper;

        public CharacterService(IMapper mapper) {
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            Character? character = characters.FirstOrDefault(c => c.Id == id);
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> SaveCharacter(AddCharacterDto characterDto)
        {
            Character character = mapper.Map<Character>(characterDto);
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }
    }

}
