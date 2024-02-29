
using AutoMapper;
using RPG_API.Dtos;
using System.Linq.Expressions;

namespace RPG_API.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private static List<Character> characters = new List<Character> {
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

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto characterDto)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try {
                Character? character = characters.FirstOrDefault(c => c.Id == characterDto.Id);
                if (character is null)
                    throw new Exception($"Character with id: {characterDto.Id} not found");

                character.Name = characterDto.Name;
                character.HitPoints = characterDto.HitPoints;
                character.Strength = characterDto.Strength;
                character.Defense = characterDto.Defense;
                character.Intelligence = characterDto.Intelligence;
                character.RpgClass = characterDto.RpgClass;

                serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            }  catch (Exception e) {
                serviceResponse.Success= false;
                serviceResponse.message= e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character? character = characters.FirstOrDefault(c => c.Id == id);
                if (character is null)
                    throw new Exception($"Character with id: {id} not found");

                characters.Remove(character);

                serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.message = e.Message;
            }
            return serviceResponse;
        }

    }

}
