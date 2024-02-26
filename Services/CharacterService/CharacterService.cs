
namespace RPG_API.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private List<Character> characters = new List<Character> {
            new Character(),
            new Character { Id = 1, Name = "Sam"}
        };

        public List<Character> GetAllCharacters()
        {
            return characters;
        }

        public Character GetCharacterById(int id)
        {
            Character? character = characters.FirstOrDefault(c => c.Id == id);
            if (character != null)
            {
                return character;
            }
            throw new Exception($"Character with id: {id} not found.");
        }

        public List<Character> SaveCharacter(Character character)
        {
            characters.Add(character);
            return characters;
        }
    }

}
