using System;

namespace SDP.TeamAlpha.Journals.Application.RegisterService
{
    public class RegisterNewUserRequest
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Company { get; set; }
    }
}
