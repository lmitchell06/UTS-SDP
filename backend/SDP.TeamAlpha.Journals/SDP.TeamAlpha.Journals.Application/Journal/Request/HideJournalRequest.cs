using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application
{
    public class HideJournalRequest
    {
        public int JournalId { get; set; }
        public bool Hidden { get; set; }
    }
}
