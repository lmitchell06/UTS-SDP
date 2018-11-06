namespace SDP.TeamAlpha.Journals.Application
{
    public class UserFactory : IUserFactory
    {
        public User Create(string username, string password, string firstName, string lastName, string company)
        {
            var user = new User
            {
                Username = username,
                Password = password,
                PersonalDetails = new UserPersonalDetails
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Company = company
                }
            };
            return user;
        }
    }
}