
namespace SDP.TeamAlpha.Journals.Application.RegisterService
{
    public interface IRegisterService
    {
        RegisterNewUserResponse RegisterUser(RegisterNewUserRequest request);
    }
}
