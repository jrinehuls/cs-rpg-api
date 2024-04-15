using RPG_API.Dtos.Weapon;

namespace RPG_API.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto weaponDto);
    }
}
