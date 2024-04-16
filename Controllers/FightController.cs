using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPG_API.Dtos.Fight;
using RPG_API.Services.FightService;

namespace RPG_API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;
        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("weapon-attack")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            var response = await _fightService.WeaponAttack(weaponAttack);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("skill-attack")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto skillAttack)
        {
            var response = await _fightService.SkillAttack(skillAttack);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("fight")]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto fightRequest)
        {
            ServiceResponse<FightResultDto> response = await _fightService.Fight(fightRequest);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("high-scores")]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDto>>>> GetHighScores()
        {
            ServiceResponse<List<HighScoreDto>> response = await _fightService.GetHighScore();
            return Ok(response);
        }
    }
}
