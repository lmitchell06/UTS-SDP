using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDP.TeamAlpha.Journals.Application.Validators;

namespace SDP.TeamAlpha.Journals.Application.RegisterService
{
    public class RegisterNewUserResponseFactory : IRegisterNewUserResponseFactory
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        public RegisterNewUserResponseFactory(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        public RegisterNewUserResponse Create(RegisterNewUserRequest request)
        {
            var response = new RegisterNewUserResponse();
            var validatorResults = new RegisterValidator(_userRepository).ValidateRegistration(request);
            if (validatorResults.All(x => x.IsValid))
            {
                var user = _userService.CreateUser(request.Username,
                    request.Password,
                    request.FirstName,
                    request.LastName,
                    request.Company);
                response.UserId = user.Id;
            }
            response.ValidationResults = validatorResults;
            return response;
        }
    }
}
