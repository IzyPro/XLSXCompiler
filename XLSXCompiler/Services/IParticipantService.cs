using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLSXCompiler.Models;
using XLSXCompiler.ViewModels;

namespace XLSXCompiler.Services
{
    public interface IParticipantService
    {
        Task<ResponseManager> EnrollParticipantsAsync(SheetDetailsViewModel file);
        Task<ResponseManager> CompileToSheetAsync(CompileViewModel file);
        Task<MeetingViewModel> SheetDetailsAsync(int sheetID);
    }
}
