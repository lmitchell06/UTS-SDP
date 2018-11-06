using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.EntryService.Request
{
    public class ListEntriesRequest
    {
        public int ParentId { get; set; }
        public bool ShowHidden { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
