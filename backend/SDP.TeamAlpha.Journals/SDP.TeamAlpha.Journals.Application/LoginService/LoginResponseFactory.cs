using System.Collections.Generic;
using System.Linq;
using SDP.TeamAlpha.Journals.Application.Validators;

namespace SDP.TeamAlpha.Journals.Application.LoginService
{
    public class LoginResponseFactory : ILoginResponseFactory
    {
        private ILoginValidator _validator;
        private IUserRepository _userRepository;
        private IUserService _userService;

        public LoginResponseFactory(ILoginValidator validator, IUserRepository repository, IUserService userService)
        {
            _validator = validator;
            _userRepository = repository;
            _userService = userService;
        }
        public LoginWithCredentialsResponse Create(string username, string password)
        {
            List<ValidatorResult> results = new List<ValidatorResult>();
            results.Add(_validator.ValidateUsername(username));
            results.Add(_validator.ValidatePassword(username, password));

            var success = results.FirstOrDefault(x => x.IsValid == false) == null;
            var response = new LoginWithCredentialsResponse
            {
                Message = success ? "Success" : "Failure",
                ValidationResults = results,
                User = success ? _userRepository.Fetch(username) : null
            };
            return response;
        }
    }
}