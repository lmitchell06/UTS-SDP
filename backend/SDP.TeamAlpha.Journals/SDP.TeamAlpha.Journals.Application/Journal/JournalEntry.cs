using System;
using System.Linq;
using System.Collections.Generic;

namespace SDP.TeamAlpha.Journals.Application
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public List<Revision> Revisions { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public int ParentId { get; set; }
        public int AuthorId { get; set; }
        public int OwnerId { get; set; }
        public bool Hidden { get; set; }
        public bool Deleted { get; set; }
        public Revision LatestRevision => Revisions.FirstOrDefault(x => x.Edited == Revisions.Max(y => y.Edited));

        public JournalEntry()
        {
            if (Revisions == null)
                Revisions = new List<Revision>();
        }
        public JournalEntry(string title, int parentId)
        {
            Created = DateTime.Now;
            Revisions = new List<Revision>();
            ParentId = parentId;
            Title = title;
            Hidden = false;
            Deleted = false;
        }
    }
}
