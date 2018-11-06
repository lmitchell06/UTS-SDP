using System.Web.Http;
using SDP.TeamAlpha.Journals.Application.LoginService;
using SDP.TeamAlpha.Journals.Application;
using System.Web;
using System.Web.SessionState;
using SDP.TeamAlpha.Journals.Services.Utils;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Http.Description;

namespace SDP.TeamAlpha.Journals.Services.Controllers
{
    public class LoginController : ApiController
    {
        private ILoginService _loginService;

        public LoginController()
        {
            
        }
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [Route("api/LoginWithCredentials"), ResponseType(typeof(LoginWithCredentialsResponse))]
        public HttpResponseMessage LoginWithCredentials(LoginWithCredentialsRequest request)
        {
            var resp = CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host);
            
            var loginResult = _loginService.LoginWithCredentials(request.Username, request.Password);
            
            resp.Content = new StringContent(
                                        JsonConvert.SerializeObject(
                                            loginResult, 
                                            Formatting.Indented));

            return resp;
        }

        [Route("api/Logout")]
        public void Logout()
        {
            CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host);

            _loginService.Logout();
        }
    }
}
