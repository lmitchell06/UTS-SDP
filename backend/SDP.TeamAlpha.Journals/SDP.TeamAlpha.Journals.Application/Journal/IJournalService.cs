using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Application
{
    public interface IJournalService
    {
        ViewJournalResponse CreateJournal(string projectName, string ownerUsername);
        ViewJournalResponse ViewJournal(int journalId);
        List<ViewJournalResponse> ListJournals(bool showHidden);
        bool HideJournal(int journalId, bool hidden);
    }
}
