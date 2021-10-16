using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class MeetingDetail
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime JoinTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public string Duration { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ParticipantID { get; set; }
    }
}
