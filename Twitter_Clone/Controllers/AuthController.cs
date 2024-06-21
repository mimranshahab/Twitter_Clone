using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter_Clone.API.Models;
using Twitter_Clone.API.Services;

namespace Twitter_Clone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _userService.Authenticate(model);

            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _userService.Register(model);

            if (!result)
                return BadRequest(new { message = "Registration failed" });

            return Ok(new { message = "Registration successful" });
        }
    }
}
