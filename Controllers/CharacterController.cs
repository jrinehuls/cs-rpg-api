using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;

namespace RPG_API.Controllers
{

    [ApiController] // @RestController
    [Route("api/[controller]")] // @Requestmapping("api/Character")
    public class CharacterController : ControllerBase
    {

        private readonly ICharacterService characterService;

        public CharacterController(ICharacterService characterService) {
            this.characterService = characterService;
        }

        // ActionResult<Character> = ResponseEntity<Character>
        [HttpGet("/all")] // @GetMapping("/all")
        public async Task<ActionResult<ServiceResponse<List<Character>>>> GetCharacters()
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Character>>> GetCharacter(int id)
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> SaveCharater(Character character)
        {
            return Ok(await characterService.SaveCharacter(character));
        }

    }

}
