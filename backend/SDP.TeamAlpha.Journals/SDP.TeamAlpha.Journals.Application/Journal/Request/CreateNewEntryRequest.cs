using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application
{
    public class CreateNewEntryRequest
    {
        public string Title { get; set; }
        public int ParentId { get; set; }
    }
}
