using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Application
{
    public class JournalFactory : IJournalFactory
    {
        private readonly IUserSession _session;
        private readonly IUserRepository _userRepository;

        public JournalFactory(IUserSession session, IUserRepository userRepository)
        {
            _session = session;
            _userRepository = userRepository;
        }

        public Journal CreateJournal(string projectName, string ownerUsername)
        {
            var ownerId = _userRepository.Fetch(ownerUsername)?.Id;
            var journal = new Journal(projectName, 
                ownerId ?? 0, 
                _session.CurrentUser.Id);
            return journal;
        }

        public ViewJournalResponse CreateViewJournalResponse(Journal journal)
        {
            if (journal.AuthorId != SessionCache.Current.CurrentUser.Id)
                return new ViewJournalResponse
                {
                    JournalId = -1
                };

            var response = new ViewJournalResponse
            {
                JournalId = journal.Id,
                ProjectName = journal.ProjectName,
                Entries = new List<ViewJournalEntryResponse>(),
                Journal = journal
            };
            foreach (JournalEntry entry in journal.Entries)
            {
                response.Entries.Add(new ViewJournalEntryResponse
                {
                    Created = entry.Created,
                    Title = entry.Title,
                    LatestRevision = entry.LatestRevision
                });
            }
            return response;
        }
    }
}
