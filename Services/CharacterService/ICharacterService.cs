using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RPG_API.Services.CharacterService
{

    public interface ICharacterService
    {
        Task<ServiceResponse<List<Character>>> GetAllCharacters();

        Task<ServiceResponse<Character>> GetCharacterById(int id);

        Task<ServiceResponse<List<Character>>> SaveCharacter(Character character);
    }

}
