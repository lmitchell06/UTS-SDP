namespace SDP.TeamAlpha.Journals.Application
{
    public class UserService : IUserService
    {
        private IUserFactory _factory;
        private IUserRepository _repository;
        private IUserSession _session;
        
        public UserService(IUserFactory factory, IUserRepository repository, IUserSession session)
        {
            _factory = factory;
            _repository = repository;
            _session = session;
        }
        
        public User CreateUser(string username, string password, string firstName, string lastName, string company)
        {
            var user = _factory.Create(username, password, firstName, lastName, company);
            _repository.Save(user);
            _session.CurrentUser = user;
            
            return user;
        }
    }
}
