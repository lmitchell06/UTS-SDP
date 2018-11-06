using System;
using System.Collections.Generic;
using SDP.TeamAlpha.Journals.Application.RegisterService;

namespace SDP.TeamAlpha.Journals.Application.Validators
{
    public class RegisterValidator
    {
        private IUserRepository _userRepository;

        public RegisterValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public List<ValidatorResult> ValidateRegistration(RegisterNewUserRequest request)
        {
            List<ValidatorResult> results = new List<ValidatorResult>();

            results.Add(ValidateName(request.FirstName, "FirstName"));
            results.Add(ValidateName(request.LastName, "LastName"));
            results.Add(ValidateUsername(request.Username));
            results.Add(ValidatePassword(request.Password));

            return results;
        }
        public ValidatorResult ValidateName(string name, string whichName)
        {
            var result = new ValidatorResult
            {
                Field = whichName
            };
            if (string.IsNullOrEmpty(name))
            {
                result.IsValid = false;
                result.Message = whichName + " field is empty";
                return result;
            }
            result.IsValid = name.Length < 32;
            result.Message = result.IsValid ? "Success" : whichName + " should be less than 32 characters";
            return result;
        }
        public ValidatorResult ValidateUsername(string username)
        {
            var result = new ValidatorResult
            {
                Field = "Username"
            };
            if (string.IsNullOrEmpty(username))
            {
                result.IsValid = false;
                result.Message = "Username field is empty";
            }
            else if (username.Length > 32) //the table stores usernames as NVARCHAR(32)
            {
                result.IsValid = false;
                result.Message = "Username should be less than 32 characters";
            }
            else
            {
                result.IsValid = !_userRepository.Exists(username);
                result.Message = result.IsValid ? "Success" : "Username is in use";
            }
            return result;
        }
        public ValidatorResult ValidatePassword(string password)
        {
            var result = new ValidatorResult
            {
                Field = "Password"
            };
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                result.IsValid = false;
                result.Message = "Password should be at least 8 characters";
            }
            else
            {
                result.IsValid = true;
                result.Message = "Success";
            }
            return result;
        }
    }
}
