namespace SDP.TeamAlpha.Journals.Application
{
    public class UserSession : IUserSession
    {
        public User CurrentUser
        {
            get
            {
                return SessionCache.Current.CurrentUser;
            }
            set
            {
                SessionCache.Current.CurrentUser = value;
            }
        }
    }
}