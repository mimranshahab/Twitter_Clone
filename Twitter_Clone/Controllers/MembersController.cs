using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Twitter_Clone.API.Data;
using Twitter_Clone.API.Models;
using Twitter_Clone.API.Utilities;

namespace Twitter_Clone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        //public MembersController(DatabaseContext context)
        //{
        //    _context = context;
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateMember(Member member)
        //{
        //    using var connection = _context.CreateConnection();
        //    var command = new SqlCommand("INSERT INTO Members (Email, Password, CreatedAt) VALUES (@Email, @Password, @CreatedAt); SELECT SCOPE_IDENTITY();", connection);
        //    command.Parameters.AddWithValue("@Email", member.Email);
        //    command.Parameters.AddWithValue("@Password", PasswordHasher.HashPassword(member.Password));
        //    command.Parameters.AddWithValue("@CreatedAt", member.CreatedAt);

        //    await connection.OpenAsync();
        //    member.MemberId = Convert.ToInt32(await command.ExecuteScalarAsync());

        //    return CreatedAtAction(nameof(GetMemberById), new { id = member.MemberId }, member);
        //}


        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetMemberById(int id)
        //{
        //    using var connection = _context.CreateConnection();
        //    var command = new SqlCommand("SELECT MemberId, Email, CreatedAt FROM Members WHERE MemberId = @Id", connection);
        //    command.Parameters.AddWithValue("@Id", id);

        //    await connection.OpenAsync();
        //    using var reader = await command.ExecuteReaderAsync();
        //    if (await reader.ReadAsync())
        //    {
        //        var member = new Member
        //        {
        //            MemberId = reader.GetInt32(0),
        //            Email = reader.GetString(1),
        //            CreatedAt = reader.GetDateTime(2)
        //        };
        //        return Ok(member);
        //    }

        //    return NotFound();
        //}


        //new  

        private readonly MemberRepository _memberRepository;

        public MembersController(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Member member)
        {
            var existingMember = await _memberRepository.GetMemberByEmailAsync(member.Email);
            if (existingMember != null)
            {
                return Ok(existingMember);
            }

            var newMember = await _memberRepository.AddMemberAsync(member.Email, member.Password);
            return Ok(newMember);
        }
    }
}
