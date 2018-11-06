using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private IRegisterNewUserResponseFactory _factory;

        public RegisterService(IRegisterNewUserResponseFactory factory)
        {
            _factory = factory;
        }
        public RegisterNewUserResponse RegisterUser(RegisterNewUserRequest request)
        {
            return _factory.Create(request);
        }
    }
}
