namespace SDP.TeamAlpha.Journals.Application
{
    public class CreateNewJournalResponse
    {
        public int JournalId { get; set; }
        public string ProjectName { get; set; }
        public int OwnerId { get; set; }
        public int AuthorId { get; set; }
    }
}
