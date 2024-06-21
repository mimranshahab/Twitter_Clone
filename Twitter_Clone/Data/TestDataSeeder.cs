using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.SqlClient;
using Twitter_Clone.API.Data;
using Twitter_Clone.API.Models;
using Twitter_Clone.API.Utilities;

namespace Twitter_Clone.API.Data
{
    public class TestDataSeeder
    {
        private readonly DatabaseContext _context;

        public TestDataSeeder(DatabaseContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            for (int i = 1; i <= 25000; i++)
            {
                var email = $"user{i}@example.com";
                var password = PasswordHasher.HashPassword("password");

                var memberCommand = _context.Connection.CreateCommand();
                memberCommand.CommandText = "INSERT INTO Members (Email, Password, CreatedAt) VALUES (@Email, @Password, @CreatedAt); SELECT SCOPE_IDENTITY();";
                memberCommand.CommandType = CommandType.Text;

                var emailParam = memberCommand.CreateParameter();
                emailParam.ParameterName = "@Email";
                emailParam.Value = email;
                memberCommand.Parameters.Add(emailParam);

                var passwordParam = memberCommand.CreateParameter();
                passwordParam.ParameterName = "@Password";
                passwordParam.Value = password;
                memberCommand.Parameters.Add(passwordParam);

                var createdAtParam = memberCommand.CreateParameter();
                createdAtParam.ParameterName = "@CreatedAt";
                createdAtParam.Value = DateTime.UtcNow;
                memberCommand.Parameters.Add(createdAtParam);

                var memberId = Convert.ToInt32(memberCommand.ExecuteScalar());

                var tweetCount = new Random().Next(0, 6);
                for (int j = 0; j < tweetCount; j++)
                {
                    var message = $"Tweet {j + 1} from {email}";

                    var tweetCommand = _context.Connection.CreateCommand();
                    tweetCommand.CommandText = "INSERT INTO Tweets (Message, PostedDate, MemberId) VALUES (@Message, @PostedDate, @MemberId)";
                    tweetCommand.CommandType = CommandType.Text;

                    var messageParam = tweetCommand.CreateParameter();
                    messageParam.ParameterName = "@Message";
                    messageParam.Value = message;
                    tweetCommand.Parameters.Add(messageParam);

                    var postedDateParam = tweetCommand.CreateParameter();
                    postedDateParam.ParameterName = "@PostedDate";
                    postedDateParam.Value = DateTime.UtcNow;
                    tweetCommand.Parameters.Add(postedDateParam);

                    var memberIdParam = tweetCommand.CreateParameter();
                    memberIdParam.ParameterName = "@MemberId";
                    memberIdParam.Value = memberId;
                    tweetCommand.Parameters.Add(memberIdParam);

                    tweetCommand.ExecuteNonQuery();
                }
            }
        }

        //public async Task Seed()
        //{
        //    using var connection = _context.CreateConnection();
        //    await connection.OpenAsync();

        //    for (int i = 1; i <= 25000; i++)
        //    {
        //        var email = $"user{i}@example.com";
        //        var password = PasswordHasher.HashPassword("password");

        //        var memberCommand = new SqlCommand("INSERT INTO Members (Email, Password, CreatedAt) VALUES (@Email, @Password, @CreatedAt); SELECT SCOPE_IDENTITY();", connection);
        //        memberCommand.Parameters.AddWithValue("@Email", email);
        //        memberCommand.Parameters.AddWithValue("@Password", password);
        //        memberCommand.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

        //        var memberId = Convert.ToInt32(await memberCommand.ExecuteScalarAsync());

        //        var tweetCount = new Random().Next(0, 6);
        //        for (int j = 0; j < tweetCount; j++)
        //        {
        //            var message = $"Tweet {j + 1} from {email}";
        //            var tweetCommand = new SqlCommand("INSERT INTO Tweets (Message, PostedDate, MemberId) VALUES (@Message, @PostedDate, @MemberId);", connection);
        //            tweetCommand.Parameters.AddWithValue("@Message", message);
        //            tweetCommand.Parameters.AddWithValue("@PostedDate", DateTime.UtcNow);
        //            tweetCommand.Parameters.AddWithValue("@MemberId", memberId);

        //            await tweetCommand.ExecuteNonQueryAsync();
        //        }
        //    }
        //}
    }
}
