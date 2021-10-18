using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLSXCompiler.Models;

namespace XLSXCompiler.Data
{
    public class XLSXContext : DbContext
    {
        public XLSXContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<SheetDetails> SheetDetails { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<MeetingParticipants> MeetingParticipants { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
    }
}
