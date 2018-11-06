using System.Collections.Generic;
using System.Linq;

namespace SDP.TeamAlpha.Journals.Application
{
    public class JournalService : IJournalService
    {
        private readonly IJournalFactory _factory;
        private readonly IJournalRepository _repository;

        public JournalService(IJournalFactory factory, IJournalRepository repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public ViewJournalResponse CreateJournal(string projectName, string ownerUsername)
        {
            var journal = _factory.CreateJournal(projectName, ownerUsername);
            _repository.Save(journal);

            var response = _factory.CreateViewJournalResponse(journal);

            return response;
        }

        public List<ViewJournalResponse> ListJournals(bool showHidden)
        {
            var list = _repository.List()
                .Where(j => j.AuthorId == SessionCache.Current.CurrentUser.Id);
            list = showHidden ? list : list.Where(j => j.Hidden == false);
            return list.Select(j => _factory.CreateViewJournalResponse(j)).ToList();
        }

        public ViewJournalResponse ViewJournal(int journalId)
        {
            var journal = _repository.Fetch(journalId);
            return _factory.CreateViewJournalResponse(journal);
        }

        public bool HideJournal(int journalId, bool hidden)
        {
            return _repository.SetHidden(journalId, hidden);
        }
    }
}
