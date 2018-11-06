namespace SDP.TeamAlpha.Journals.Application
{
    public interface IUserFactory
    {
        User Create(string username, string password, string firstName, string lastName, string company);
    }
}