using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Application.EntryService
{
    public interface IEntryRepository
    {
        JournalEntry Fetch(int entryId);
        void Save(JournalEntry entry);
        List<JournalEntry> ListEntries(int journalId, bool showHidden);
        bool SetHidden(JournalEntry entry, bool hidden);
        bool Delete(JournalEntry entry);
    }
}
