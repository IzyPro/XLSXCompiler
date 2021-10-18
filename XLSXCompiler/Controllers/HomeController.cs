using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XLSXCompiler.Data;
using XLSXCompiler.Models;
using XLSXCompiler.Services;
using XLSXCompiler.ViewModels;

namespace XLSXCompiler.Controllers
{
    public class HomeController : Controller
    {
        private IParticipantService _participantService;
        private readonly ILogger<HomeController> _logger;
        private XLSXContext _context;

        private static List<SheetDetails> sheets;

        public HomeController(ILogger<HomeController> logger, IParticipantService participantService, XLSXContext context)
        {
            _logger = logger;
            _participantService = participantService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sheets = await _context.SheetDetails.ToListAsync();
            return View(sheets);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Enrollment([FromForm] SheetDetailsViewModel model)
        {
            var result = await _participantService.EnrollParticipantsAsync(model);
            if (result.isSuccess)
                TempData["EnrollmentSuccess"] = result.Message;
            else
                TempData["EnrollmentError"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CompileToSheet([FromForm] CompileViewModel model)
        {
            var result = await _participantService.CompileToSheetAsync(model);
            if (result.isSuccess)
                TempData["CompileSuccess"] = result.Message;
            else
                TempData["CompileError"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Meetings()
        {
            sheets = await _context.SheetDetails.ToListAsync();
            return View(sheets);
        }

        [HttpPost]
        public async Task<IActionResult> Meetings(int sheetID)
        {
            var meetingViewModel = await _participantService.SheetDetailsAsync(sheetID);
            ViewBag.MeetingVM = meetingViewModel;
            return View(sheets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
