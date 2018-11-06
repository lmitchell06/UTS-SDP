using SDP.TeamAlpha.Journals.Application.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP.TeamAlpha.Journals.Application.Data
{
    public class Context : DbContext
    {
        public Context() : base("Connection")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Revision> Revisions { get; set; }
        //public DbSet<Session> Sessions { get; set; }
    }
}
