namespace SDP.TeamAlpha.Journals.Application
{
    public interface IJournalFactory
    {
        Journal CreateJournal(string projectName, string ownerUsername);
        ViewJournalResponse CreateViewJournalResponse(Journal journal);
    }
}
