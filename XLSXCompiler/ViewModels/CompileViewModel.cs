using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.ViewModels
{
    public class CompileViewModel
    {
        public int SheetID { get; set; }
        public DateTime Date { get; set; }
        public IFormFile file { get; set; }
    }
}
