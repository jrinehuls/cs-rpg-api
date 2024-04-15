
using AutoMapper;
using System.Linq.Expressions;
using System.Security.Claims;

namespace RPG_API.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper mapper;

        private readonly DataContext context;

        private readonly IHttpContextAccessor httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor) {
            this.mapper = mapper;
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            List<Character> characters = await context.Character
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            Character? character = await context.Character
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> SaveCharacter(AddCharacterDto characterDto)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = mapper.Map<Character>(characterDto)!;

            character.User = await context.Users.FirstOrDefaultAsync(user => user.Id == GetUserId());

            context.Character.Add(character);
            await context.SaveChangesAsync();

            serviceResponse.Data = await context.Character
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => mapper.Map<GetCharacterDto>(c)!)
                .ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto characterDto)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try {
                Character? character = await context.Character
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Include(c => c.User) // If not, the user will be null. It won't get the related user from the users table
                    .FirstOrDefaultAsync(c => c.Id == characterDto.Id);
                if (character is null || character.User.Id != GetUserId())
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
                Character? character = await context.Character
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (character is null)
                    throw new Exception($"Character with id: {id} not found");

                context.Character.Remove(character);

                await context.SaveChangesAsync();

                serviceResponse.Data = await context.Character
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => mapper.Map<GetCharacterDto>(c)!).ToListAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto characterSkillDto)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try {

                Character? character = await context.Character
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == characterSkillDto.CharacterId &&
                    c.User!.Id == GetUserId());
                if (character is null)
                {
                    throw new Exception($"Character with id {characterSkillDto.CharacterId} not found");
                }

                Skill? skill = await context.Skill.FindAsync(characterSkillDto.SkillId);

                if (skill is null)
                {
                    throw new Exception($"Skill with id {characterSkillDto.SkillId} not found");
                }

                character.Skills!.Add(skill);
                await context.SaveChangesAsync();
                response.Data = mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e) {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }


        // Note: In this case, User is property of ControllerBase, not User model
        private int GetUserId() => int.Parse(httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

}
