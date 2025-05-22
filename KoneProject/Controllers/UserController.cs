using KoneProject.DTOs;
using KoneProject.Helpers;
using KoneProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using KoneProject.Authorisations;


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
        public async Task<ActionResult<ApiRessponse>> Register([FromBody] UserDto userDto)
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
        public async Task<ActionResult<ApiRessponse>> Login([FromBody] loginDto logDto)
        {
            // Appel de la méthode Login du UserService
            var result = await _userService.Login(logDto);
            if (result.Success)
            {
                return Ok(new ApiRessponse<IEnumerable<UserDto>>(true,"Utilisateur connecté avec succè",result));
            }
            return BadRequest(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ApiRessponse>> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            return result.Success ? Ok(new ApiRessponse<IEnumerable<UserDto>>(true,"",result)) : NotFound(result);
        }
    }
}
