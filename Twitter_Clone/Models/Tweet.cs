namespace Twitter_Clone.API.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string Message { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
