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
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetCharacters()
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacter(int id)
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> SaveCharater(AddCharacterDto character)
        {
            return Ok(await characterService.SaveCharacter(character));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharater(UpdateCharacterDto character)
        {
            ServiceResponse<GetCharacterDto> response = await characterService.UpdateCharacter(character);
            if (response.Data is null) 
            { 
                return NotFound(response); 
            }
            return Ok(response);
        }

    }

}
