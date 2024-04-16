using AutoMapper;
using RPG_API.Dtos.Fight;

namespace RPG_API.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                if (attacker is null)
                {
                    throw new Exception($"Character with Id: {weaponAttack.AttackerId} not found");
                }
                else if (opponent is null)
                {
                    throw new Exception($"Character with Id: {weaponAttack.OpponentId} not found");
                }

                int damage = DoWeaponAttack(attacker, opponent);

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

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                Character? attacker = await _context.Character
                    .Include(a => a.Skills)
                    .FirstOrDefaultAsync(a => a.Id == skillAttack.AttackerId);
                Character? opponent = await _context.Character
                    .FindAsync(skillAttack.OpponentId);
                // Null checks
                if (attacker is null)
                {
                    throw new Exception($"Character with Id: {skillAttack.AttackerId} not found");
                }
                else if (opponent is null)
                {
                    throw new Exception($"Character with Id: {skillAttack.OpponentId} not found");
                }
                else if (attacker.Skills is null || attacker.Skills.Count <= 0)
                {
                    throw new Exception($"Character with Id: {skillAttack.OpponentId} has no skills");
                }

                Skill? skill = attacker.Skills.FirstOrDefault(s => s.Id == skillAttack.SkillId);

                if (skill is null)
                {
                    throw new Exception($"Character with Id: {skillAttack.OpponentId} " +
                        $"doesn't have skill with id: {skillAttack.SkillId}");
                }

                int damage = DoSkillAttack(attacker, opponent, skill);

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

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fightRequest)
        {
            var response = new ServiceResponse<FightResultDto>();
            response.Data = new FightResultDto();

            try
            {
                List<Character> characters = await _context.Character
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => fightRequest.CharacterIds.Contains(c.Id))
                    .ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach(var attacker in characters)
                    {
                        List<Character> opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        Character opponent = opponents[new Random().Next(opponents.Count)];

                        int damage;
                        string attackUsed;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon && attacker.Weapon is not null)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else if (!useWeapon && attacker.Skills is not null && attacker.Skills.Count > 0)
                        {
                            Skill skill = attacker.Skills.ToList()[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        else
                        {
                            response.Data.Log.Add($"{attacker.Name} wasn't able to attack");
                            continue;
                        }
                        response.Data.Log
                            .Add($"{attacker.Name} used {attackUsed} and dealt {damage} damage to {opponent.Name}");
                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{attacker.Name} defeated {opponent.Name}");
                            break;
                        }
                    }
                }
                characters.ForEach(c => 
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });

                await _context.SaveChangesAsync();
            } 
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var response = new ServiceResponse<List<HighScoreDto>>();

            List<Character> characters = await _context.Character
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();

            List<HighScoreDto> highScores = characters.Select(c => _mapper.Map<HighScoreDto>(c)!).ToList();

            response.Data = highScores;

            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if (attacker.Weapon is null)
            {
                throw new Exception($"Character with Id: {attacker.Id} has no weapon");
            }
            int damage = attacker.Weapon.Damage + new Random().Next(attacker.Strength);
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }
            else
            {
                damage = 0;
            }

            return damage;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + new Random().Next(attacker.Intelligence);
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            } 
            else
            {
                damage = 0;
            }

            return damage;
        }

    }
}
