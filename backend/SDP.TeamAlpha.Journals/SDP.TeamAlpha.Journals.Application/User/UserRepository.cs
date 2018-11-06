using SDP.TeamAlpha.Journals.Application.Data;
using System.Collections.Generic;
using System.Linq;

namespace SDP.TeamAlpha.Journals.Application
{
    public class UserRepository : IUserRepository
    {
        public virtual List<User> Users { get; set; }
        public User Fetch(string username)
        {
            using (var ctx = new Context())
            {
                return ctx.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
            }
        }
        
        public User Fetch(int id)
        {
            using (var ctx = new Context())
            {
                return ctx.Users.FirstOrDefault(x => x.Id == id);
            }
        }

        public List<User> List()
        {
            using (var ctx = new Context())
            {
                return ctx.Users.ToList();
            }
        }

        public bool Exists(string username)
        {
            return Fetch(username.ToLower()) != null;
        }

        public bool PasswordMatches(string username, string password)
        {
            using (var ctx = new Context())
            {
                return ctx.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower()
                                                  && x.Password == password) 
                                                  != null;
            }
        }

        public void Save(User user)
        {
            using (var ctx = new Context())
            {
                if (ctx.Users.FirstOrDefault(u => u.Id == user.Id) == null)
                    ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var user = Fetch(id);
            using (var ctx = new Context())
            {
                if (user != null)
                    ctx.Users.Remove(ctx.Users.First(x => x.Id == id));
                ctx.SaveChanges();
            }
        }
    }
}