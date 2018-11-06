using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.EntryService.Request
{
    public class EditEntryRequest
    {
        public int EntryId { get; set; }
        public string Body { get; set; }
    }
}
