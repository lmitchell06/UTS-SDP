using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SDP.TeamAlpha.Journals.Services.Controllers
{
    public class OptionController : ApiController
    {
        public OptionController()
        {

        }
        [HttpOptions]
        public HttpResponseMessage Option()
        {
            var response = new HttpResponseMessage();
                response.Content = new StringContent("OK");
            return response;
        }
    }
}