using Korty.Data;
using Korty.DTOs;
using Korty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Korty.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public async Task<IActionResult> Index(DateTime? date, int? startHour, int? endHour)
        {
            var checkDate = date ?? DateTime.Today;
            
            var courts = await _context.Courts
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var availabilityList = new List<CourtAvailabilityDTO>();

            foreach (var court in courts)
            {
                var openingHour = court.OpeningHour;
                var closingHour = court.ClosingHour;

                var reservations = await _context.Reservations
                    .Where(r => r.CourtId == court.Id
                        && r.Date.Date == checkDate.Date
                        && r.Status == "Active")
                    .ToListAsync();

                reservations = reservations
                    .Where(r => r.EndTime > openingHour && r.StartTime < closingHour)
                    .OrderBy(r => r.StartTime)
                    .ToList();

                var availableRanges = new List<TimeRangeDTO>();
                var reservedRanges = reservations.Select(r => new TimeRangeDTO
                {
                    StartTime = r.StartTime,
                    EndTime = r.EndTime
                }).ToList();

                var currentStart = openingHour;

                foreach (var reservation in reservations)
                {
                    // Adding available time range if the current start is before the reservation start
                    if (currentStart < reservation.StartTime)
                    {
                        availableRanges.Add(new TimeRangeDTO
                        {
                            StartTime = currentStart,
                            EndTime = reservation.StartTime
                        });
                    }
                    currentStart = reservation.EndTime > currentStart ? reservation.EndTime : currentStart;
                }

                // Adding available time range if the current start is before the closing hour
                if (currentStart < closingHour)
                {
                    availableRanges.Add(new TimeRangeDTO
                    {
                        StartTime = currentStart,
                        EndTime = closingHour
                    });
                }

                availabilityList.Add(new CourtAvailabilityDTO
                {
                    CourtId = court.Id,
                    CourtName = court.Name,
                    Date = checkDate,
                    AvailableTimeRanges = availableRanges,
                    ReservedTimeRanges = reservedRanges,
                    OpeningHour = openingHour,
                    ClosingHour = closingHour
                });
            }

            var baseEarliestOpeningHour = courts.Any() ? courts.Min(a => a.OpeningHour) : new TimeOnly(6, 0, 0);
            var baseLatestClosingHour = courts.Any() ? courts.Max(a => a.ClosingHour) : new TimeOnly(23, 0, 0);
            
            var earliestOpeningHour = startHour.HasValue ? new TimeOnly(startHour.Value, 0, 0) : baseEarliestOpeningHour;
            var latestClosingHour = endHour.HasValue ? new TimeOnly(endHour.Value, 0, 0) : baseLatestClosingHour;
            
            ViewBag.SelectedDate = checkDate;
            ViewBag.AvailabilityList = availabilityList;
            ViewBag.EarliestOpeningHour = earliestOpeningHour;
            ViewBag.LatestClosingHour = latestClosingHour;
            ViewBag.BaseEarliestOpeningHour = baseEarliestOpeningHour;
            ViewBag.BaseLatestClosingHour = baseLatestClosingHour;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
