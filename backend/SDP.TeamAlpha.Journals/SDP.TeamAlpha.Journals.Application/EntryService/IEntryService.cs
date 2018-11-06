using System;
using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Application.EntryService
{
    public interface IEntryService
    {
        Revision EditEntry(string body, int entryId);
        JournalEntry CreateEntry(string title, int journalId, string body);
        List<JournalEntry> ListEntries(int parentId, bool showHidden, DateTime? startDate, DateTime? endDate);
        bool ToggleHidden(int entryId, bool hidden);
        List<JournalEntry> SearchEntries(int journalId, bool showHidden, string inBody = null, string name = null, DateTime? startDate = null, DateTime? endDate = null);
        bool DeleteEntry(int entryId);
    }
}
