using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace SDP.TeamAlpha.Journals.Services.Controllers
{
    public class ControllerBase : ApiController
    {
        protected HttpResponseMessage PrepareResponse(HttpResponseMessage resp, object content)
        {
            resp.Content = new StringContent(
                JsonConvert.SerializeObject(
                    content,
                    Formatting.Indented));

            return resp;
        }
    }
}