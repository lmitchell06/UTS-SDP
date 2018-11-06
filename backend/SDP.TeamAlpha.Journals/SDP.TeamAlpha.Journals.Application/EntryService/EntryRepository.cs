using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using SDP.TeamAlpha.Journals.Application.Data;

namespace SDP.TeamAlpha.Journals.Application.EntryService
{
    public class EntryRepository : IEntryRepository
    {
        public JournalEntry Fetch(int entryId)
        {
            using (var ctx = new Context())
            {
                return ctx.JournalEntries.FirstOrDefault(e => e.Id == entryId);
            }
        }

        public List<JournalEntry> ListEntries(int parentId, bool showHidden)
        {
            using (var ctx = new Context())
            {
                var entries = ctx.JournalEntries.Where(e => e.ParentId == parentId && !e.Deleted).ToList();
                foreach (var entry in entries)
                {
                    entry.Revisions = GetRevisions(entry.Id);
                }
                return showHidden ? entries.Where(e => !e.Hidden).ToList() : entries;
            }
        }

        public List<Revision> GetRevisions(int entryId)
        {
            using (var ctx = new Context())
            {
                return ctx.Revisions.Where(e => e.ParentId == entryId).ToList();
            }
        }

        public bool SetHidden(JournalEntry entry, bool hidden)
        {
            JournalEntry journalEntry;
            using (var ctx = new Context())
            {
                if (ctx.JournalEntries.FirstOrDefault(e => e.Id == entry.Id) == null)
                {
                    ctx.JournalEntries.Add(entry);
                }
                journalEntry = ctx.JournalEntries.FirstOrDefault(e => e.Id == entry.Id);
            }

            if (journalEntry != null)
            {
                journalEntry.Hidden = hidden;

                using (var ctx = new Context())
                {
                    ctx.Entry(journalEntry).State = EntityState.Modified;
                    foreach (var revision in entry.Revisions)
                    {
                        if (ctx.Revisions.FirstOrDefault(r => r.Id == revision.Id) == null)
                            ctx.Revisions.Add(revision);
                    }
                    ctx.SaveChanges();
                }
            }
            return journalEntry != null;
        }

        public bool Delete(JournalEntry entry)
        {
            JournalEntry journalEntry;
            using (var ctx = new Context())
            {
                if (ctx.JournalEntries.FirstOrDefault(e => e.Id == entry.Id) == null)
                {
                    ctx.JournalEntries.Add(entry);
                }
                journalEntry = ctx.JournalEntries.FirstOrDefault(e => e.Id == entry.Id);
            }

            if (journalEntry != null)
            {
                journalEntry.Deleted = true;

                using (var ctx = new Context())
                {
                    ctx.Entry(journalEntry).State = EntityState.Modified;
                    foreach (var revision in entry.Revisions)
                    {
                        if (ctx.Revisions.FirstOrDefault(r => r.Id == revision.Id) == null)
                            ctx.Revisions.Add(revision);
                    }
                    ctx.SaveChanges();
                }
            }
            return journalEntry != null;
        }

        public void Save(JournalEntry entry)
        {
            using (var ctx = new Context())
            {
                if (ctx.JournalEntries.FirstOrDefault(e => e.Id == entry.Id) == null)
                {
                    ctx.JournalEntries.Add(entry);
                }

                foreach (var revision in entry.Revisions)
                {
                    if (ctx.Revisions.FirstOrDefault(r => r.Id == revision.Id) == null)
                        ctx.Revisions.Add(revision);
                }
                ctx.SaveChanges();
            }
        }
    }
}
