using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Twitter_Clone.API.Data;
using Twitter_Clone.API.Models;

namespace Twitter_Clone.API.Data
{
    public class TweetRepository
    {
        private readonly DatabaseContext _context;

        public TweetRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Tweet> AddTweetAsync(string message, int memberId)
        {
            var query = "INSERT INTO Tweets (Message, PostedDate, MemberId) OUTPUT INSERTED.TweetId VALUES (@Message, @PostedDate, @MemberId)";
            using (var command = _context.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                var messageParam = command.CreateParameter();
                messageParam.ParameterName = "@Message";
                messageParam.Value = message;
                command.Parameters.Add(messageParam);

                var postedDateParam = command.CreateParameter();
                postedDateParam.ParameterName = "@PostedDate";
                postedDateParam.Value = DateTime.UtcNow;
                command.Parameters.Add(postedDateParam);

                var memberIdParam = command.CreateParameter();
                memberIdParam.ParameterName = "@MemberId";
                memberIdParam.Value = memberId;
                command.Parameters.Add(memberIdParam);

                var tweetId = (int) command.ExecuteScalar();
                return new Tweet
                {
                    TweetId = tweetId,
                    Message = message,
                    PostedDate = DateTime.UtcNow,
                    MemberId = memberId
                };
            }
        }

        public async Task<List<Tweet>> GetTweetsAsync()
        {
            var query = "SELECT TOP 10 TweetId, Message, PostedDate, MemberId FROM Tweets ORDER BY PostedDate DESC";
            var tweets = new List<Tweet>();

            using (var command = _context.Connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tweets.Add(new Tweet
                        {
                            TweetId = reader.GetInt32(0),
                            Message = reader.GetString(1),
                            PostedDate = reader.GetDateTime(2),
                            MemberId = reader.GetInt32(3)
                        });
                    }
                }
            }

            return tweets;
        }
    }
}
