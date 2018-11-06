using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application
{
    public class Revision
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public DateTime Edited { get; set; }
        public string Body { get; set; }
    }
}
