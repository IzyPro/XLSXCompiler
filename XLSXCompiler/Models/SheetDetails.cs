using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class SheetDetails
    {
        public int Id { get; set; }
        [Required]
        public string ProgramName { get; set; }
        [Required]
        public List<Participant> Participants { get; set; }
    }
}
