
namespace SDP.TeamAlpha.Journals.Application.LoginService
{
    public interface ILoginService
    {
        LoginWithCredentialsResponse LoginWithCredentials(string username, string password);
        void Logout();
    }
}
