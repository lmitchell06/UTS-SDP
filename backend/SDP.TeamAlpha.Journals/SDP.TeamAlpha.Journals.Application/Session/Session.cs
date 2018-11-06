using System;
using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Application
{
    public class Session
    {
        public string Id { get; set; }
        public User CurrentUser { get; set; }
        public DateTime LastAccessed { get; set; }

        public Session(string id)
        {
            Id = id;
            LastAccessed = DateTime.Now;
        }
    }

    public static class SessionCache
    {
        public static Session Current;
        static Dictionary<string, Session> Sessions = new Dictionary<string, Session>();

        public static Session GetItem(string sessionId)
        {
            Session session;
            if (!Sessions.TryGetValue(sessionId, out session))
            {
                session = new Session(sessionId);
                Sessions.Add(sessionId, session);
            }
            return session;
        }

        public static void NewSession(string sessionId, Session session)
        {
            Sessions.Add(sessionId, session);
            Current = session;
        }
    }
}