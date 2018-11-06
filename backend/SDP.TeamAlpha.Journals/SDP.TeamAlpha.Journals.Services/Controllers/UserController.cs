using Newtonsoft.Json;
using SDP.TeamAlpha.Journals.Application;
using SDP.TeamAlpha.Journals.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SDP.TeamAlpha.Journals.Services.Controllers
{
    public class UserController : ApiController
    {
        private IUserRepository _userRepository;
        private IUserSession _userSession;

        public UserController() { }//obligatory ctor

        public UserController(IUserRepository userRepository, IUserSession userSession)
        {
            _userRepository = userRepository;
            _userSession = userSession;
        }

        [Route("api/User"), ResponseType(typeof(User))]
        public HttpResponseMessage GetCurrentUser()
        {
            var resp = CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host);

            resp.Content = new StringContent(
                                        JsonConvert.SerializeObject(
                                            _userSession.CurrentUser,
                                            Formatting.Indented));

            return resp;
        }

        [Route("api/User/DeleteUser")]
        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }

        [Route("api/User/UserExists"), ResponseType(typeof(bool?))]
        public HttpResponseMessage UserExists(string username)
        {
            var resp = CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host);

            bool? value = null;
            // only logged in users can look it up.
            if (_userSession.CurrentUser != null) value = _userRepository.Fetch(username) != null;

            resp.Content = new StringContent(
                                        JsonConvert.SerializeObject(
                                            value,
                                            Formatting.Indented));

            return resp;
        }

        [Route("api/User/ListUsers"), ResponseType(typeof(List<User>))]
        public HttpResponseMessage ListUsers()
        {
            var resp = CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host);
            
            var users = _userRepository.List();

            resp.Content = new StringContent(
                                        JsonConvert.SerializeObject(
                                            users, 
                                            Formatting.Indented));

            return resp;
        }
    }
}
