using System.Net.Http;
using System.Web.Http;
using SDP.TeamAlpha.Journals.Application.EntryService;
using SDP.TeamAlpha.Journals.Application.EntryService.Request;
using SDP.TeamAlpha.Journals.Services.Utils;
using System.Web.Http.Description;
using SDP.TeamAlpha.Journals.Application;
using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Services.Controllers
{
    public class EntryController : ControllerBase
    {
        private readonly IEntryService _entryService;

        #region Constructors
        public EntryController() { }
        public EntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }
        #endregion

        [Route("api/Entry/Create"), ResponseType(typeof(JournalEntry))]
        public HttpResponseMessage CreateNewEntry(CreateEntryRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host), 
                _entryService.CreateEntry(request.Title, request.ParentId, request.Body));
        }

        [Route("api/Entry/Edit"), ResponseType(typeof(Revision))]
        public HttpResponseMessage EditEntry(EditEntryRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _entryService.EditEntry(request.Body, request.EntryId));
        }

        [Route("api/Entry/List"), ResponseType(typeof(List<JournalEntry>))]
        public HttpResponseMessage ListEntries(ListEntriesRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host), 
                _entryService.ListEntries(request.ParentId, request.ShowHidden, request.StartDate, request.EndDate));
        }

        [Route("api/Entry/ToggleHidden"), ResponseType(typeof(bool))]
        public HttpResponseMessage ToggleHidden(ToggleHiddenRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _entryService.ToggleHidden(request.EntryId, request.Hidden));
        }

        [Route("api/Entry/Search"), ResponseType(typeof(List<JournalEntry>))]
        public HttpResponseMessage SearchEntries(SearchEntryRequest request)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _entryService.SearchEntries(request.JournalId, request.ShowHidden, request.InBody, request.Name, request.StartDate, request.EndDate));
        }

        [Route("api/Entry/Delete")]
        public HttpResponseMessage DeleteEntry(int entryId)
        {
            return PrepareResponse(
                CookieHandler.HandleCookies(Request.Headers, Request.RequestUri.Host),
                _entryService.DeleteEntry(entryId));
        }
    }
}
