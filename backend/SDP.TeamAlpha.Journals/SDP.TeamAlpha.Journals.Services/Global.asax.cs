using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace SDP.TeamAlpha.Journals.Services
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DependencyConfig.RegisterTypes();
        }

        protected void Application_BeginRequest( object sender, EventArgs e)
        {
            var origin = Request.Headers["Origin"];
            if (origin == null) origin = Request.Url.GetLeftPart(UriPartial.Authority);
            Response.Headers.Add("Access-Control-Allow-Origin", origin);
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, HEAD, DELETE, POST, PUT, PATCH");
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
        }
    }
}
