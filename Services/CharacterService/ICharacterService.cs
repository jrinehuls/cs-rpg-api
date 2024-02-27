using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RPG_API.Services.CharacterService
{

    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();

        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);

        Task<ServiceResponse<List<GetCharacterDto>>> SaveCharacter(AddCharacterDto character);
    }

}
