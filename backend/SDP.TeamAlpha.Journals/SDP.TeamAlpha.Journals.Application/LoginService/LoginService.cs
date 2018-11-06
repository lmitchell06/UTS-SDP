using System;
using SDP.TeamAlpha.Journals.Application.Validators;

namespace SDP.TeamAlpha.Journals.Application.LoginService
{
    public class LoginService : ILoginService
    {
        private ILoginValidator _validator;
        private ILoginResponseFactory _loginResponseFactory;
        private IUserSession _userSession;
        private IUserRepository _userRepository;
        
        public LoginService(ILoginResponseFactory loginResponseFactory, ILoginValidator validator, IUserSession userSession, IUserRepository userRepository)
        {
            _loginResponseFactory = loginResponseFactory;
            _validator = validator;
            _userSession = userSession;
            _userRepository = userRepository;
        }

        public LoginWithCredentialsResponse LoginWithCredentials(string username, string password)
        {
            //Validate username/password and create response.
            var response = _loginResponseFactory.Create(username, password);
            if (response.User != null)
            {
                SessionCache.Current.CurrentUser = response.User;
            }
            return response;
        }

        public void Logout()
        {
            SessionCache.Current.CurrentUser = null;
        }
    }
}
