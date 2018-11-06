namespace SDP.TeamAlpha.Journals.Application
{
    public interface IUserService
    {
        User CreateUser(string username, string password, string firstName, string lastName, string company);
    }
}