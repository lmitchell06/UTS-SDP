using SDP.TeamAlpha.Journals.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace SDP.TeamAlpha.Journals.Services.Utils
{
    public class CookieHandler
    {
        public static HttpResponseMessage HandleCookies(HttpRequestHeaders headers, string host)
        {
            var sessionCookie = headers.GetCookies("session-id")
                                       .FirstOrDefault();
            
            var response = new HttpResponseMessage();

            if (sessionCookie == null)
            {
                //create a new cookie to post to user
                var guid = GenerateGuid();
                var cookie = new CookieHeaderValue("session-id", guid);
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = host;
                cookie.Path = "/";

                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                
                SessionCache.NewSession(guid, new Session(guid));

                return response;
            }

            var sessionId = sessionCookie.Cookies[0].Value;
            SessionCache.Current = SessionCache.GetItem(sessionId);
            return response;
        }

        private static string GenerateGuid()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }
    }
}