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
        public ActionResult<List<Character>> GetCharacters()
        {
            return Ok(characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetCharacter(int id)
        {
            return Ok(characterService.GetCharacterById(id));
        }

        [HttpPost]
        public ActionResult<List<Character>> SaveCharater(Character character)
        {
            return Ok(characterService.SaveCharacter(character));
        }

    }

}
