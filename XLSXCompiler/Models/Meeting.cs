﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.Models
{
    public class Meeting
    {
        public Guid Id { get; set; }
        public int SheetID { get; set; }
        public DateTime Date { get; set; }
    }
}
