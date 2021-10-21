using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class Meeting
    {
        public Guid Id { get; set; }
        [Required]
        public int SheetID { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
