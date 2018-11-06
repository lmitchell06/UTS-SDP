namespace SDP.TeamAlpha.Journals.Application
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public bool AccountConfirmed { get; set; }
        public UserPersonalDetails PersonalDetails { get; set; }
    }
}
