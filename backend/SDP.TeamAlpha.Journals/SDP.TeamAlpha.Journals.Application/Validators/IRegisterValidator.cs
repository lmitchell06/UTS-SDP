using SDP.TeamAlpha.Journals.Application.RegisterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.Validators
{
    public interface IRegisterValidator
    {
        List<ValidatorResult> ValidateRegistration(RegisterNewUserRequest request);
        ValidatorResult ValidateName(string name, string whichName);
        ValidatorResult ValidateUsername(string username);
        ValidatorResult ValidatePassword(string password);
    }
}
