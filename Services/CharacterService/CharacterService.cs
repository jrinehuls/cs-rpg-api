
using AutoMapper;
using System.Linq.Expressions;

namespace RPG_API.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper mapper;

        private readonly DataContext context;

        public CharacterService(IMapper mapper, DataContext context) {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            List<Character> characters = await context.Character.ToListAsync();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            Character? character = await context.Character.FirstOrDefaultAsync(c => c.Id == id);
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> SaveCharacter(AddCharacterDto characterDto)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            Character? character = mapper.Map<Character>(characterDto);
            context.Character.Add(character);
            await context.SaveChangesAsync();

            serviceResponse.Data = await context.Character.Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto characterDto)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try {
                Character? character = await context.Character.FirstOrDefaultAsync(c => c.Id == characterDto.Id);
                if (character is null)
                {
                    throw new Exception($"Character with id: {characterDto.Id} not found");
                }

                character.Name = characterDto.Name;
                character.HitPoints = characterDto.HitPoints;
                character.Strength = characterDto.Strength;
                character.Defense = characterDto.Defense;
                character.Intelligence = characterDto.Intelligence;
                character.RpgClass = characterDto.RpgClass;

                await context.SaveChangesAsync();

                serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            }  catch (Exception e) {
                serviceResponse.Success= false;
                serviceResponse.Message= e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character? character = await context.Character.FirstOrDefaultAsync(c => c.Id == id);
                if (character is null)
                    throw new Exception($"Character with id: {id} not found");

                context.Character.Remove(character);

                await context.SaveChangesAsync();

                serviceResponse.Data = await context.Character.Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

    }

}
