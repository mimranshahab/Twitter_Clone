using Moq;
using System.Threading.Tasks;
using Twitter_Clone.API.Models;
using Twitter_Clone.API.Services;
using Twitter_Clone.API.Utilities;
using Twitter_Clone.API.Data;
using Xunit;

namespace TwitterClone.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<MemberRepository> _memberRepositoryMock;
        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _memberRepositoryMock = new Mock<MemberRepository>();
        }

        [Fact]
        public async Task Register_ShouldReturnTrue_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerModel = new RegisterModel
            {
                Password = "Test@1234",
                Email = "testuser@example.com"
            };

            _memberRepositoryMock.Setup(repo => repo.AddMemberAsync(It.IsAny<Member>())
                                 .ReturnsAsync(true));

            var result = await _userService.Register(registerModel);

            Assert.True(result);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var loginModel = new LoginModel
            {
                Email = "testuser",
                Password = "Test@1234"
            };

            var member = new Member
            {
                MemberId = 1,
                Password = PasswordHasher.HashPassword("Test@1234"),
                Email = "testuser@example.com"
            };

            _memberRepositoryMock.Setup(repo => repo.GetMemberByEmailAsync(loginModel.Email))
                                 .ReturnsAsync(member);

            var token = await _userService.Authenticate(loginModel);

            Assert.NotNull(token);
        }

        [Fact]
        public async Task Authenticate_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            var loginModel = new LoginModel
            {
                Email = "testuser",
                Password = "WrongPassword"
            };

            var member = new Member
            {
                MemberId = 1,
                Password = PasswordHasher.HashPassword("Test@1234"),
                Email = "testuser@example.com"
            };

            _memberRepositoryMock.Setup(repo => repo.GetMemberByEmailAsync(loginModel.Email))
                                 .ReturnsAsync(member);

            var token = await _userService.Authenticate(loginModel);

            Assert.Null(token);
        }
    }
}
