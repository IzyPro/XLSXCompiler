using Ganss.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XLSXCompiler.Data;
using XLSXCompiler.Models;
using XLSXCompiler.ViewModels;

namespace XLSXCompiler.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private XLSXContext _context;

        public ParticipantService(IWebHostEnvironment hostEnvironment, XLSXContext context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        public async Task<ResponseManager> CompileToSheetAsync(CompileViewModel model)
        {
            try
            {

                var file = model.file;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath, $"files{Path.PathSeparator}{fileName}");
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();
                    if (!File.Exists(path))
                        return new ResponseManager
                        {
                            isSuccess = false,
                            Message = "Unable to access file",
                        };
                    using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var excel = new ExcelMapper();
                        var attendees = (await excel.FetchAsync<AttendeesDetail>(stream)).ToList();

                        var attendeesAboveTimeLimit = new List<AttendeesDetail>();
                        if (attendees == null)
                            return new ResponseManager
                            {
                                isSuccess = false,
                                Message = "Unable to read entries from sheet or sheet is empty",
                            };

                        for (int i = 0; i < attendees.Count; i++)
                        {
                            if ((attendees[i].LeaveTime - attendees[i].JoinTime).TotalMinutes > 30)
                            {
                                attendeesAboveTimeLimit.Add(attendees[i]);
                            }
                        }

                        var participants = await _context.Participants.ToListAsync();

                        var details = new Meeting
                        {
                            Id = Guid.NewGuid(),
                            SheetID = model.SheetID,
                            Date = model.Date,
                        };
                        var meetingParticipants = new List<MeetingParticipants>();
                        foreach (var entry in participants)
                        {
                            if (attendeesAboveTimeLimit.FirstOrDefault(x => x.FullName?.ToLower() == entry.FullName.ToLower()) != null ||
                                attendeesAboveTimeLimit.FirstOrDefault(x => x.Email?.ToLower() == entry.EmailAddress.ToLower()) != null ||
                                attendeesAboveTimeLimit.FirstOrDefault(x => x.FullName?.ToLower() == entry.EmailAddress.ToLower()) != null
                                || attendeesAboveTimeLimit.FirstOrDefault(x => x.FullName.ToLower().Contains(entry.FullName.ToLower())) != null
                                )

                                meetingParticipants.Add(new MeetingParticipants
                                {
                                    MeetingId = details.Id,
                                    ParticipantID = entry.ParticipantId
                                });
                        }

                        await _context.Meetings.AddAsync(details);
                        await _context.MeetingParticipants.AddRangeAsync(meetingParticipants);
                        var result = await _context.SaveChangesAsync();

                        if (result > 0)
                        {
                            return new ResponseManager
                            {
                                isSuccess = true,
                                Message = "File parsed successfully!"
                            };
                        }
                        else
                        {
                            return new ResponseManager
                            {
                                isSuccess = false,
                                Message = "Unable to save details to database. Try Again",
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseManager
                {
                    isSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ResponseManager> EnrollParticipantsAsync(SheetDetailsViewModel model)
        {
            try
            {
                var file = model.file;
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath, $"files{Path.PathSeparator}{fileName}");
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();
                    if (!File.Exists(path))
                        return new ResponseManager
                        {
                            isSuccess = false,
                            Message = "Unable to access file",
                        };
                    using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        var excel = new ExcelMapper();
                        var participants = (await excel.FetchAsync<Participant>(stream)).ToList();

                        if (participants == null)
                            return new ResponseManager
                            {
                                isSuccess = false,
                                Message = "Unable to read entries from sheet or sheet is empty",
                            };

                        var details = new SheetDetails
                        {
                            ProgramName = model.ProgramName,
                            Participants = participants
                        };

                        await _context.Participants.AddRangeAsync(participants);
                        await _context.SheetDetails.AddAsync(details);
                        var result = await _context.SaveChangesAsync();

                        if (result > 0)
                        {
                            return new ResponseManager
                            {
                                isSuccess = true,
                                Message = "File parsed successfully!"
                            };
                        }
                        else
                        {
                            return new ResponseManager
                            {
                                isSuccess = false,
                                Message = "Unable to save details to database. Try Again",
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseManager
                {
                    isSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<MeetingViewModel> SheetDetailsAsync(int sheetID)
        {
            var sheet = await _context.SheetDetails.Include(x => x.Participants).FirstOrDefaultAsync(x => x.Id == sheetID);
            var meetings = await _context.Meetings.Where(x => x.SheetID == sheetID).ToListAsync();
            var meetingDetails = new List<MeetingDetails>();

            foreach (var meeting in meetings)
            {
                var participants = await _context.MeetingParticipants.Where(x => x.MeetingId == meeting.Id).ToListAsync();
                var details = new MeetingDetails
                {
                    SheetID = meeting.SheetID,
                    Date = meeting.Date,
                    Participants = participants
                };
                meetingDetails.Add(details);
            }

            var result = new MeetingViewModel
            {
                SheetDetails = sheet,
                Meetings = meetingDetails.OrderBy(x => x.Date).ToList(),
            };
            return result;
        }
    }
}
