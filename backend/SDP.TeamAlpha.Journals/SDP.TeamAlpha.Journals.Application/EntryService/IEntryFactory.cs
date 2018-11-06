namespace SDP.TeamAlpha.Journals.Application.EntryService
{
    public interface IEntryFactory
    {
        JournalEntry CreateEntry(string title, int parentId, string body);
    }
}
