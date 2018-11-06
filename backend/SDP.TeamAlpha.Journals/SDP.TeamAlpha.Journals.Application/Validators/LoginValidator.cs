using System;

namespace SDP.TeamAlpha.Journals.Application.Validators
{
    public class LoginValidator : ILoginValidator
    {
        private IUserRepository _userRepository;
        public LoginValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            else
            {
                result.IsValid = _userRepository.Exists(username);
                result.Message = result.IsValid ? "Success" : "User does not exist";
            }
            return result;
        }
        public ValidatorResult ValidatePassword(string username, string password)
        {
            var result = new ValidatorResult
            {
                Field = "Password"
            };
            if (string.IsNullOrEmpty(password))
            {
                result.IsValid = false;
                result.Message = "Enter a password";
            }
            else
            {
                result.IsValid = _userRepository.PasswordMatches(username, password);
                result.Message = result.IsValid ? "Success" : "Username and password do not match";
            }
            return result;
        }
    }
}
