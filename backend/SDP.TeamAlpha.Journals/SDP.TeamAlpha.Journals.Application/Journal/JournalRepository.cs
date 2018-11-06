using System.Collections.Generic;
using System.Data.Entity;
using SDP.TeamAlpha.Journals.Application.Data;
using SDP.TeamAlpha.Journals.Application.EntryService;
using System.Linq;

namespace SDP.TeamAlpha.Journals.Application
{
    public class JournalRepository : IJournalRepository
    {
        private readonly IEntryRepository _entryRepo;

        public JournalRepository(IEntryRepository entryRepo)
        {
            _entryRepo = entryRepo;
        }

        public void Save(Journal journal)
        {
            using (var ctx = new Context())
            {
                if (ctx.Journals.FirstOrDefault(j => j.Id == journal.Id) == null)
                    ctx.Journals.Add(journal);
                
                foreach (var entry in journal.Entries)
                {
                    if (ctx.JournalEntries.FirstOrDefault(e => e.Id == entry.Id) == null)
                        _entryRepo.Save(entry);
                }
                ctx.SaveChanges();
            }
        }

        public bool SetHidden(int id, bool hidden)
        {
            Journal journal;
            using (var ctx = new Context())
            {
                journal = ctx.Journals.FirstOrDefault(e => e.Id == id);
            }
            if (journal != null)
            {
                journal.Hidden = hidden;

                using (var ctx = new Context())
                {
                    ctx.Entry(journal).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
            }
            return journal != null;
        }

        public Journal Fetch(int journalId)
        {
            using (var ctx = new Context())
            {
                var journal = ctx.Journals.FirstOrDefault(j => j.Id == journalId);
                if (journal != null)
                    journal.Entries = _entryRepo.ListEntries(journalId, false);

                return journal;
            }
        }

        public List<Journal> List()
        {
            using (var ctx = new Context())
            {
                var journals = ctx.Journals.Where(x => x.AuthorId == SessionCache.Current.CurrentUser.Id);
                foreach (var journal in journals)
                    journal.Entries = _entryRepo.ListEntries(journal.Id, false);

                return journals.ToList();
            }
        }
    }
}
