namespace Twitter_Clone.API.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
