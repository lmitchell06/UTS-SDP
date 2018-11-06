using System;
using System.Collections.Generic;
using System.Linq;

namespace SDP.TeamAlpha.Journals.Application
{
    public class ProjectJournal
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public List<JournalEntry> Entries { get; set; }
        public User Owner { get; set; }
        public User Author { get; set; }

        public ProjectJournal(string projectName, User owner, User author)
        {
            ProjectName = projectName;
            Owner = owner;
            Author = author;
            Entries = new List<JournalEntry>();
        }
    }
}
