using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Twitter_Clone.API.Data;
using Twitter_Clone.API.Models;

namespace Twitter_Clone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly TweetRepository _tweetRepository;
        private readonly TestDataSeeder _testDataSeeder;
        private readonly MemberRepository _memberRepository;

        public TweetsController(TweetRepository tweetRepository, MemberRepository memberRepository, TestDataSeeder testDataSeeder)
        {
            _tweetRepository = tweetRepository;
            _memberRepository = memberRepository;
            _testDataSeeder = testDataSeeder;
        }

        [HttpGet]
        public async Task<IActionResult> GetTweets()
        {
            var tweets = await _tweetRepository.GetTweetsAsync();
            //await _testDataSeeder.Seed();
            return Ok(tweets);
        }

        [HttpPost]
        public async Task<IActionResult> PostTweet([FromBody] Tweet tweet)
        {
            var member = await _memberRepository.GetMemberByIDAsync(tweet.MemberId.ToString());
            if (member == null)
            {
                return BadRequest("Invalid member.");
            }

            var newTweet = await _tweetRepository.AddTweetAsync(tweet.Message, tweet.MemberId);
            return Ok(newTweet);
        }
        //public TweetsController(DatabaseContext context)
        //{
        //    _context = context;
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateTweet(Tweet tweet)
        //{
        //    if (string.IsNullOrWhiteSpace(tweet.Message) || tweet.Message.Length > 140)
        //    {
        //        return BadRequest("Tweet message is invalid.");
        //    }

        //    using var connection = _context.CreateConnection();
        //    var command = new SqlCommand("INSERT INTO Tweets (Message, PostedDate, MemberId) VALUES (@Message, @PostedDate, @MemberId); SELECT SCOPE_IDENTITY();", connection);
        //    command.Parameters.AddWithValue("@Message", tweet.Message);
        //    command.Parameters.AddWithValue("@PostedDate", tweet.PostedDate);
        //    command.Parameters.AddWithValue("@MemberId", tweet.MemberId);

        //    await connection.OpenAsync();
        //    tweet.TweetId = Convert.ToInt32(await command.ExecuteScalarAsync());

        //    return CreatedAtAction(nameof(GetTweetById), new { id = tweet.TweetId }, tweet);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetTweetById(int id)
        //{
        //    using var connection = _context.CreateConnection();
        //    var command = new SqlCommand("SELECT TweetId, Message, PostedDate, MemberId FROM Tweets WHERE TweetId = @Id", connection);
        //    command.Parameters.AddWithValue("@Id", id);

        //    await connection.OpenAsync();
        //    using var reader = await command.ExecuteReaderAsync();
        //    if (await reader.ReadAsync())
        //    {
        //        var tweet = new Tweet
        //        {
        //            TweetId = reader.GetInt32(0),
        //            Message = reader.GetString(1),
        //            PostedDate = reader.GetDateTime(2),
        //            MemberId = reader.GetInt32(3)
        //        };
        //        return Ok(tweet);
        //    }

        //    return NotFound();
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetRecentTweets()
        //{
        //    using var connection = _context.CreateConnection();
        //    var command = new SqlCommand("SELECT TOP 10 TweetId, Message, PostedDate, MemberId FROM Tweets ORDER BY PostedDate DESC", connection);

        //    await connection.OpenAsync();
        //    using var reader = await command.ExecuteReaderAsync();
        //    var tweets = new List<Tweet>();
        //    while (await reader.ReadAsync())
        //    {
        //        tweets.Add(new Tweet
        //        {
        //            TweetId = reader.GetInt32(0),
        //            Message = reader.GetString(1),
        //            PostedDate = reader.GetDateTime(2),
        //            MemberId = reader.GetInt32(3)
        //        });
        //    }

        //    return Ok(tweets);
        //}
    }
}
