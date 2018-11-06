using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.EntryService
{
    public class EntryService : IEntryService
    {
        private readonly IEntryFactory _factory;
        private readonly IEntryRepository _repository;

        public EntryService(IEntryFactory factory, IEntryRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public JournalEntry CreateEntry(string title, int journalId, string body)
        {
            var entry = _factory.CreateEntry(title, journalId, body);
            _repository.Save(entry);
            return entry;
        }

        public bool ToggleHidden(int entryId, bool hidden)
        {
            var entry = _repository.Fetch(entryId);
            if (entry != null)
            {
                entry.Hidden = hidden;
                _repository.SetHidden(entry, hidden);
            }
            return entry != null;
        }

        public Revision EditEntry(string body, int entryId)
        {
            var entry = _repository.Fetch(entryId);
            if (entry != null)
            {
                var revision = new Revision
                {
                    Body = body,
                    ParentId = entry.Id,
                    Edited = DateTime.Now
                };
                entry.Revisions.Add(revision);
                _repository.Save(entry);
                return revision;
            }
            return null;
        }

        public List<JournalEntry> ListEntries(int parentId, bool showHidden, DateTime? startDate = null, DateTime? endDate = null)
        {
            var entries = _repository.ListEntries(parentId, showHidden);
            if (startDate.HasValue && !endDate.HasValue)
            {
                return entries.Where(e => e.LatestRevision.Edited.Date == startDate.Value.Date).ToList();
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                return entries.Where(e => e.LatestRevision.Edited.Date > startDate.Value.Date 
                                       && e.LatestRevision.Edited.Date < endDate.Value.Date)
                                    .ToList();
            }
            return entries;
        }

        public List<JournalEntry> SearchEntries(int journalId, bool showHidden, string inBody = null, string name = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var entries = ListEntries(journalId, showHidden, startDate, endDate);

            if (!string.IsNullOrEmpty(inBody))
                entries = entries.Where(e => 
                    e.LatestRevision != null && 
                    e.LatestRevision.Body.Contains(inBody)).ToList();

            if (!string.IsNullOrEmpty(name))
                entries = entries.Where(e => e.Title == name).ToList();

            return entries;
        }

        public bool DeleteEntry(int entryId)
        {
            var entry = _repository.Fetch(entryId);
            if (entry != null)
            {
                _repository.Delete(entry);
                return true;
            }
            return false;
        }
    }
}
