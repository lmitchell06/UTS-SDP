using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.EntryService.Response
{
    public class EntryDetailsResponse
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public bool Hidden { get; set; }
        public string Title { get; set; }
        public RevisionDetailsResponse LatestRevision { get; set; }
    }

    public class RevisionDetailsResponse
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime Updated { get; set; }
    }
}
