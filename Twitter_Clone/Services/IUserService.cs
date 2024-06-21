using Twitter_Clone.API.Models;

namespace Twitter_Clone.API.Services
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginModel model);
        Task<bool> Register(RegisterModel model);
    }
}
