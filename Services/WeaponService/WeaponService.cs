using AutoMapper;
using RPG_API.Dtos.Weapon;
using System.Security.Claims;

namespace RPG_API.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {

        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        // Add weapon and assign to character
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto weaponDto)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                // Find character with same c.ID as weapon and same u.id as user
                Character? character = await _context.Character
                    .FirstOrDefaultAsync(c => c.Id == weaponDto.CharacterId && 
                    c.User!.Id == int.Parse(_contextAccessor.HttpContext!.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!));
                if (character is null)
                {
                    throw new Exception($"Character with Id: {weaponDto.CharacterId} not found");
                }
                // Map dto to weapon
                Weapon weapon = _mapper.Map<Weapon>(weaponDto)!;
                // Save weapon
                _context.Weapon.Add(weapon);
                await _context.SaveChangesAsync();
                // Set response data
                response.Data = _mapper.Map<GetCharacterDto>(character);
            } catch (Exception e) {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
    }
}
