﻿using RPG_API.Dtos.Fight;

namespace RPG_API.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                Character? attacker = await _context.Character
                    .Include(a => a.Weapon)
                    .FirstOrDefaultAsync(a => a.Id == weaponAttack.AttackerId);
                Character? opponent = await _context.Character
                    .FindAsync(weaponAttack.OpponentId);
                // Null checks
                if (attacker is null) {
                    throw new Exception($"Character with Id: {weaponAttack.AttackerId} not found");
                }
                else if (opponent is null)
                {
                    throw new Exception($"Character with Id: {weaponAttack.OpponentId} not found");
                }
                else if (attacker.Weapon is null)
                {
                    throw new Exception($"Character with Id: {weaponAttack.OpponentId} has no weapon");
                }
                int damage = attacker.Weapon.Damage + new Random().Next(attacker.Strength);
                damage -= new Random().Next(opponent.Defense);
                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }
                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated";
                }

                await _context.SaveChangesAsync();

                AttackResultDto result = new()
                {
                    Attacker = attacker.Name,
                    AttackerHp = attacker.HitPoints,
                    Opponent = opponent.Name,
                    OpponentHp = opponent.HitPoints,
                    Damage = damage
                };
                response.Data = result;
            } 
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
    }
}
