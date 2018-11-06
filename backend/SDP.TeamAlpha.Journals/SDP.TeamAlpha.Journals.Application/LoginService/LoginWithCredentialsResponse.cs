
using System.Collections.Generic;
using SDP.TeamAlpha.Journals.Application.Validators;

namespace SDP.TeamAlpha.Journals.Application.LoginService
{
    public class LoginWithCredentialsResponse
    {
        public List<ValidatorResult> ValidationResults { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
