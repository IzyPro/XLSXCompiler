using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XLSXCompiler.ViewModels
{
    public class SheetDetailsViewModel
    {
        public string ProgramName { get; set; }
        public IFormFile file { get; set; }
    }
}
