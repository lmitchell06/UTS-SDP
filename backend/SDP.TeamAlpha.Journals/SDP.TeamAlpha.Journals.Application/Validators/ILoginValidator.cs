using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.Validators
{
    public interface ILoginValidator
    {
        ValidatorResult ValidateUsername(string username);
        ValidatorResult ValidatePassword(string username, string password);
    }
}
