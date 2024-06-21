using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Twitter_Clone.API.Data;
using Twitter_Clone.API.Models;
using Twitter_Clone.API.Utilities;

namespace Twitter_Clone.API.Data
{
    public class MemberRepository
    {
        private readonly DatabaseContext _context;

        public MemberRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Member> AddMemberAsync(string email, string password)
        {
            var query = "INSERT INTO Members (Email, Password, CreatedAt) OUTPUT INSERTED.MemberId VALUES (@Email, @Password, @CreatedAt)";
            password = PasswordHasher.HashPassword(password);   
            using (var command = _context.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                var emailParam = command.CreateParameter();
                emailParam.ParameterName = "@Email";
                emailParam.Value = email;
                command.Parameters.Add(emailParam);

                var passwordParam = command.CreateParameter();
                passwordParam.ParameterName = "@Password";
                passwordParam.Value = password;
                command.Parameters.Add(passwordParam);

                var createdAtParam = command.CreateParameter();
                createdAtParam.ParameterName = "@CreatedAt";
                createdAtParam.Value = DateTime.UtcNow;
                command.Parameters.Add(createdAtParam);

                var memberId = (int) command.ExecuteScalar();
                return new Member
                {
                    MemberId = memberId,
                    Email = email,
                    Password = password,
                    CreatedAt = DateTime.UtcNow
                };
            }
        }

        public async Task<Member> ValidateMemberAsync(string email, string password)
        {
            var query = "SELECT MemberId, Email, Password, CreatedAt FROM Members WHERE Email = @email and Password = @password";
            password = PasswordHasher.HashPassword(password);
            using (var command = _context.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                var emailParam = command.CreateParameter();
                emailParam.ParameterName = "@email"; 
                emailParam.Value = email;
                command.Parameters.Add(emailParam);

                var passwordParam = command.CreateParameter();
                passwordParam.ParameterName = "@password"; 
                passwordParam.Value = password;
                command.Parameters.Add(passwordParam);

                using (var reader = command.ExecuteReader()) 
                {
                    if (reader.Read()) 
                    {
                        return new Member
                        {
                            MemberId = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3)
                        };
                    }
                }
            }
            return null; // Return null if no member was found
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            var query = "SELECT MemberId, Email, Password, CreatedAt FROM Members WHERE Email = @Email";
            using (var command = _context.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                var emailParam = command.CreateParameter();
                emailParam.ParameterName = "@Email";
                emailParam.Value = email;
                command.Parameters.Add(emailParam);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Member
                        {
                            MemberId = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3)
                        };
                    }
                }
            }

            return null;
        }
        public async Task<Member> GetMemberByIDAsync(string MemberId)
        {
            var query = "SELECT MemberId, Email, Password, CreatedAt FROM Members WHERE MemberId = @MemberId";
            using (var command = _context.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                var memberIdParam = command.CreateParameter();
                memberIdParam.ParameterName = "@MemberId";
                memberIdParam.Value = MemberId;
                command.Parameters.Add(memberIdParam);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Member
                        {
                            MemberId = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3)
                        };
                    }
                }
            }

            return null;
        }
    }
}
