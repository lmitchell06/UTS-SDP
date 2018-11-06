using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.EntryService.Request
{
    public class CreateEntryRequest
    {
        public string Title { get; set; }
        public int ParentId { get; set; }
        public string Body { get; set; }
    }
}
