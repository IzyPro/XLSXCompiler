using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class AttendeesDetail
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime JoinTime { get; set; }
        [Required]
        public DateTime LeaveTime { get; set; }
        public string Duration { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ParticipantID { get; set; }
    }
}
