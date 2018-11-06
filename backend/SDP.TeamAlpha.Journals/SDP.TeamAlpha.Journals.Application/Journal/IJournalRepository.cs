using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Application
{
    public interface IJournalRepository
    {
        void Save(Journal journal);
        Journal Fetch(int journalId);
        List<Journal> List();
        bool SetHidden(int journalId, bool hidden);
    }
}
