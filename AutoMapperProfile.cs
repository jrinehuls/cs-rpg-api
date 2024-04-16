using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RPG_API.Dtos.Character;
using RPG_API.Dtos.Skill;
using RPG_API.Dtos.Weapon;
using RPG_API.Dtos.Fight;

namespace RPG_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            // <From, To>
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<AddWeaponDto, Weapon>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighScoreDto>();
        }
    }
}
