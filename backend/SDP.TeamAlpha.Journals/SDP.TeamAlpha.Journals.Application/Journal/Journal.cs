using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP.TeamAlpha.Journals.Application
{
    public class Journal
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public List<JournalEntry> Entries { get; set; }
        public int OwnerId { get; set; }
        public int AuthorId { get; set; }
        public bool Hidden { get; set; }

        public Journal()
        {
            if (Entries == null)
                Entries = new List<JournalEntry>();
        }

        public Journal(string projectName, int ownerId, int authorId)
        {
            ProjectName = projectName;
            OwnerId = ownerId;
            AuthorId = authorId;
            Entries = new List<JournalEntry>();
            Hidden = false;
        }
    }
}
