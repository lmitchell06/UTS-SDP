using SDP.TeamAlpha.Journals.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SDP.TeamAlpha.Journals.Services.Utils;
using System.Web.Http.Description;

namespace SDP.TeamAlpha.Journals.Services.Controllers
{
    public class JournalController : ControllerBase
    {
        private IJournalService _journalService;

        public JournalController()
        {

        }
        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        [Route("api/Journal/Create/"), ResponseType(typeof(ViewJournalResponse))]
        public HttpResponseMessage CreateJournal(CreateNewJournalRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _journalService.CreateJournal(request.ProjectName, request.OwnerUsername));
        }

        [Route("api/Journal/List"), ResponseType(typeof(List<ViewJournalResponse>))]
        public HttpResponseMessage ListJournals(bool showHidden)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _journalService.ListJournals(showHidden));
        }

        [Route("api/Journal/SetHidden"), ResponseType(typeof(bool))]
        public HttpResponseMessage HideJournal(HideJournalRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _journalService.HideJournal(request.JournalId, request.Hidden));
        }

        [Route("api/Journal/View"), ResponseType(typeof(ViewJournalResponse))]
        public HttpResponseMessage ViewJournal(ViewJournalRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _journalService.ViewJournal(request.JournalId));
        }
    }
}
