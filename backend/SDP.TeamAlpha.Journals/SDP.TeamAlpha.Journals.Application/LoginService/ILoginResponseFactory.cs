using SDP.TeamAlpha.Journals.Application.Validators;

namespace SDP.TeamAlpha.Journals.Application.LoginService
{
    public interface ILoginResponseFactory
    {
        LoginWithCredentialsResponse Create(string username, string password);
    }
}