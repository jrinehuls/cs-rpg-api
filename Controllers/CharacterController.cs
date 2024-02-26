using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using RPG_API.Models;

namespace RPG_API.Controllers
{

    [ApiController] // @RestController
    [Route("api/[controller]")] // @Requestmapping("api/Character")
    public class CharacterController : ControllerBase
    {
        private List<Character> characters = new List<Character> {
            new Character(),
            new Character { Id = 1, Name = "Sam"}
        };

        // ActionResult<Character> = ResponseEntity<Character>
        [HttpGet("/all")] // @GetMapping("/all")
        public ActionResult<List<Character>> GetCharacters()
        {
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public ActionResult<Character> GetCharacter(int id)
        {
            return Ok(characters.FirstOrDefault(c => c.Id == id));
        }

    }

}
