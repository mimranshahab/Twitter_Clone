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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userService.Authenticate(model);
            if (token == null)
            {
                return Unauthorized();
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Set to true in production to ensure the cookie is only sent over HTTPS
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Register(model);
            if (!result)
            {
                return BadRequest(new { message = "Registration failed. Email might already be taken." });
            }

            return Ok(new { message = "Registration successful" });
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var token = await _userService.Authenticate(model);

        //    if (token == null)
        //        return Unauthorized();

        //    return Ok(new { Token = token });
        //}
        // In AuthController.cs
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var token = await _userService.Authenticate(model);
        //    if (token == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true, // Set to true in production to ensure the cookie is only sent over HTTPS
        //        SameSite = SameSiteMode.Strict
        //    };

        //    Response.Cookies.Append("jwt", token, cookieOptions);

        //    return Ok(new { message = "Login successful" });
        //}


        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    var result = await _userService.Register(model);

        //    if (!result)
        //        return BadRequest(new { message = "Registration failed" });

        //    return Ok(new { message = "Registration successful" });
        //}
    }
}
