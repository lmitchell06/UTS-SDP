using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SDP.TeamAlpha.Journals.Application.RegisterService;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using SDP.TeamAlpha.Journals.Services.Utils;
using SDP.TeamAlpha.Journals.Application;
using System.Web.Http.Description;

namespace SDP.TeamAlpha.Journals.Services.Controllers
{
    public class RegisterController : ApiController
    {
        private IRegisterService _registerService;

        public RegisterController(){ }//obligatory ctor

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [Route("api/Register"), ResponseType(typeof(RegisterNewUserResponse))]
        public HttpResponseMessage RegisterNewUser(RegisterNewUserRequest request)
        {
            var resp = CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host);

            var registerResult = _registerService.RegisterUser(request);           
            
            resp.Content = new StringContent(
                                        JsonConvert.SerializeObject(
                                            registerResult, 
                                            Formatting.Indented));

            return resp;
        }
    }
}
