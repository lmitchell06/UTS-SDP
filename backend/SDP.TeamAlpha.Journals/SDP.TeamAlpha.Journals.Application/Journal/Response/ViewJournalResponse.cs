using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application
{
    public class ViewJournalResponse
    {
        public int JournalId { get; set; }
        public string ProjectName { get; set; }
        public List<ViewJournalEntryResponse> Entries { get; set; }

        public Journal Journal { get; set; }
    }

    public class ViewJournalEntryResponse
    {
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public Revision LatestRevision { get; set; }
    }
}
