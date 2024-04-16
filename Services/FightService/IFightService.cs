using RPG_API.Dtos.Fight;

namespace RPG_API.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack);
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fightRequest);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();
    }
}
