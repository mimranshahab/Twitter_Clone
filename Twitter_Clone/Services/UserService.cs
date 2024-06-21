using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Twitter_Clone.API.Models;
using Twitter_Clone.API.Data;
using Twitter_Clone.API.Services;

namespace TwitterClone.API.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly string _secret;
        private readonly MemberRepository _memberRepository;

        public UserService(DatabaseContext context, MemberRepository memberRepository)
        {
            _context = context;
            _secret = "YourVeryLongSecretKeyThatIsAtLeast32Characters"; // Store this in a secure place, e.g., appsettings.json or environment variable

            _memberRepository = memberRepository;
        }

        public async Task<string> Authenticate(LoginModel model)
        {
            // Validate the user credentials here (e.g., check the database)
            var user = await _memberRepository.ValidateMemberAsync(model.Email, model.Password);   

            // For the sake of example, we'll assume a user is always valid
            //var user = await Task.FromResult(true); // Replace with actual user validation

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, model.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> Register(RegisterModel model)
        {
            // Add user registration logic here (e.g., save to the database)
            var newUser = await _memberRepository.AddMemberAsync(model.Email,model.Password);

            if (newUser == null)
                return false;

            // For the sake of example, we'll assume registration is always successful
            //return await Task.FromResult(true); // Replace with actual registration logic

            return true;
        }
    }
}
