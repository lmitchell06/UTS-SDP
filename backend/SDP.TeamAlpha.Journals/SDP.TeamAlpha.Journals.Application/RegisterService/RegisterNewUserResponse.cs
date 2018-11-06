
using System.Collections.Generic;
using SDP.TeamAlpha.Journals.Application.Validators;

namespace SDP.TeamAlpha.Journals.Application.RegisterService
{
    public class RegisterNewUserResponse
    {
        public List<ValidatorResult> ValidationResults { get; set; }
        public int UserId { get; set; }
    }
}
