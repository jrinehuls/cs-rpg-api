
namespace RPG_API.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private List<Character> characters = new List<Character> {
            new Character(),
            new Character { Id = 1, Name = "Sam"}
        };

        public async Task<ServiceResponse<List<Character>>> GetAllCharacters()
        {
            ServiceResponse<List<Character>> serviceResponse = new ServiceResponse<List<Character>>();
            serviceResponse.Data= characters;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Character>> GetCharacterById(int id)
        {
            Character? character = characters.FirstOrDefault(c => c.Id == id);
            ServiceResponse<Character> serviceResponse = new ServiceResponse<Character>();
            serviceResponse.Data = character;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Character>>> SaveCharacter(Character character)
        {
            characters.Add(character);
            ServiceResponse<List<Character>> serviceResponse = new ServiceResponse<List<Character>>();
            serviceResponse.Data = characters;
            return serviceResponse;
        }
    }

}
