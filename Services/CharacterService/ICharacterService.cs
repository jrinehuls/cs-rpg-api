using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RPG_API.Services.CharacterService
{

    public interface ICharacterService
    {
        List<Character> GetAllCharacters();

        Character GetCharacterById(int id);

        List<Character> SaveCharacter(Character character);
    }

}
