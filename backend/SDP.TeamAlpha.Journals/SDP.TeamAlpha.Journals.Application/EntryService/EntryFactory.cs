using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.EntryService
{
    public class EntryFactory : IEntryFactory
    {
        private IJournalRepository _journalRepository;
        private readonly IEntryRepository _entryRepository;

        public EntryFactory(IJournalRepository journalRepository, IEntryRepository entryRepository)
        {
            _journalRepository = journalRepository;
            _entryRepository = entryRepository;
        }

        public JournalEntry CreateEntry(string title, int parentId, string body)
        {
            var entry = new JournalEntry(title, parentId);
            var journal = _journalRepository.Fetch(parentId);
            journal.Entries.Add(entry);
            _journalRepository.Save(journal);
            entry.Revisions.Add(new Revision
            {
                Body = body,
                ParentId = entry.Id,
                Edited = DateTime.Now
            });
            _entryRepository.Save(entry);
            return entry;
        }
    }
}
