using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPG_API.Dtos.User;

namespace RPG_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto userDto)
        {
            var response = await _authRepository.Register(
                new User { Username = userDto.Username }, userDto.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response); // Could be Created(location, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto userDto)
        {
            ServiceResponse<string> response = await _authRepository.Login(userDto.Username, userDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
