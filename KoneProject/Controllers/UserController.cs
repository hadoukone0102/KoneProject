using KoneProject.DTOs;
using KoneProject.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Mes variables
        private readonly UserInterface _userService;

        // Constructeur
        public UserController(UserInterface userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            // Appel de la méthode Register du UserService
            var result = await _userService.Register(userDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] loginDto logDto)
        {
            // Appel de la méthode Login du UserService
            var result = await _userService.Login(logDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
