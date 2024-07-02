using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twitter_Clone.API.Controllers;
using Twitter_Clone.API.Models;
using Twitter_Clone.API.Services;
using System.Threading.Tasks;

namespace TwitterClone.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _configurationMock = new Mock<IConfiguration>();
            _authController = new AuthController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            var loginModel = new LoginModel { Email = "test@example.com", Password = "Password1!" };
            _userServiceMock.Setup(s => s.Authenticate(It.IsAny<LoginModel>())).ReturnsAsync("fake-jwt-token");

            var result = await _authController.Login(loginModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Login successful", ((dynamic)okResult.Value).message);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            var loginModel = new LoginModel { Email = "test@example.com", Password = "wrong-password" };
            _userServiceMock.Setup(s => s.Authenticate(It.IsAny<LoginModel>())).ReturnsAsync((string)null);

            var result = await _authController.Login(loginModel);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Register_ValidModel_ReturnsOkResult()
        {
            var registerModel = new RegisterModel
            {
                Email = "test@example.com",
                Password = "Password1!"
            };
            _userServiceMock.Setup(s => s.Register(It.IsAny<RegisterModel>())).ReturnsAsync(true);

            var result = await _authController.Register(registerModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Registration successful", ((dynamic)okResult.Value).message);
        }

        [Fact]
        public async Task Register_InvalidModel_ReturnsBadRequest()
        {
            var registerModel = new RegisterModel
            {
                Email = "invalid-email",
                Password = "short"
            };
            _authController.ModelState.AddModelError("Email", "Invalid email format");

            var result = await _authController.Register(registerModel);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
