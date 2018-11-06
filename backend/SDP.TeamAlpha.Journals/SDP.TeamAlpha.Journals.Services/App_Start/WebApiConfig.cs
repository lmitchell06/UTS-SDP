using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SDP.TeamAlpha.Journals.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var constraints = new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(System.Net.Http.HttpMethod.Options) };
            config.Routes.MapHttpRoute("OPTIONS", "{*pathInfo}", new { controller = "option" }, constraints);
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}