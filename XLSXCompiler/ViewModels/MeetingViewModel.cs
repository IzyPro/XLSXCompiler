using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLSXCompiler.Models;

namespace XLSXCompiler.ViewModels
{
    public class MeetingViewModel
    {
        public SheetDetails SheetDetails { get; set; }
        public List<Meeting> Meetings { get; set; }
    }
}
