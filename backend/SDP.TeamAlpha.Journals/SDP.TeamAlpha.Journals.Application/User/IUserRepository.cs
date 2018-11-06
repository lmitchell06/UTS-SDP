using System.Collections.Generic;
using System.Security.Cryptography;

namespace SDP.TeamAlpha.Journals.Application
{
    public interface IUserRepository
    {
        User Fetch(string username);
        User Fetch(int id);
        void Save(User user);
        List<User> List();
        void Delete(int id);
        bool Exists(string username);
        bool PasswordMatches(string username, string password);
    }
}