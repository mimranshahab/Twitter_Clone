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
        
    }
}
