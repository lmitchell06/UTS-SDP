using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.EntryService.Request
{
    public class SearchEntryRequest
    {
        public int JournalId { get; set; }
        public bool ShowHidden { get; set; }
        public string InBody { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
