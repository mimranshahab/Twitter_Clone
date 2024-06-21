using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Twitter_Clone.API.Services;

namespace Twitter_Clone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authService;

        public AuthenticationController(AuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginRequest request)
        {
            if (_authService.ValidateUser(request.Email, request.Password))
            {
                return Ok("Authentication successful");
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
