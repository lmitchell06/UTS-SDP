namespace SDP.TeamAlpha.Journals.Application
{
    public interface IUserSession
    {
        User CurrentUser { get; set; }
    }
}