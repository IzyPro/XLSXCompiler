using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class MeetingParticipants
    {
        public int Id { get; set; }

        public int ParticipantID { get; set; }
        public Guid MeetingId { get; set; }
    }
}
