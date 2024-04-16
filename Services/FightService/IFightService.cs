using RPG_API.Dtos.Fight;

namespace RPG_API.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack);
    }
}
