using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class Participant
    {
        public int Id { get; set; }
        [Required]
        public int ParticipantId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string EmailAddress2 { get; set; }
    }
}
