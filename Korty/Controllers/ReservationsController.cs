using Korty.Data;
using Korty.DTOs;
using Korty.Models;
using Korty.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Korty.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public ReservationsController(
            ApplicationDbContext context, 
            UserManager<AppUser> userManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var reservations = await _context.Reservations
                .Include(r => r.Court)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();
            
            reservations = reservations
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.StartTime)
                .ToList();

            var reservationDTOs = reservations.Select(r => new ReservationDTO(r)).ToList();
            return View(reservationDTOs);
        }

        // GET: Reservations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetUserId();

            var reservation = await _context.Reservations
                .Include(r => r.Court)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            if (reservation.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var reservationDTO = new ReservationDTO(reservation);
            return View(reservationDTO);
        }

        // GET: Reservations/Create
        [Authorize]
        public async Task<IActionResult> Create(int? courtId, DateTime? date, string startTime, string endTime)
        {
            var userId = GetUserId();

            var activeCourts = await _context.Courts
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.CourtId = new SelectList(activeCourts, "Id", "Name", courtId);
            ViewBag.SelectedDate = date;

            var model = new CreateReservationDTO
            {
                CourtId = courtId ?? 0,
                Date = date ?? DateTime.Today,
                StartTime = startTime,
                EndTime = endTime
            };

            return View(model);
        }

        // POST: Reservations/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourtId,Date,StartTime,EndTime")] CreateReservationDTO reservationDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!TryParseTime(reservationDTO.StartTime, out var startTime))
            {
                ModelState.AddModelError("StartTime", "Nieprawidłowy format godziny rozpoczęcia.");
            }

            if (!TryParseTime(reservationDTO.EndTime, out var endTime))
            {
                ModelState.AddModelError("EndTime", "Nieprawidłowy format godziny zakończenia.");
            }

            var court = await _context.Courts.Where(c => c.Id == reservationDTO.CourtId && c.IsActive).FirstOrDefaultAsync();
            if (court == null)
            {
                ModelState.AddModelError("CourtId", "Wybrany kort nie istnieje.");
            }

            if (ModelState.IsValid)
            {
                var reservation = new Reservation
                {
                    CourtId = reservationDTO.CourtId,
                    UserId = user.Id,
                    Date = reservationDTO.Date,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = "Active",
                    CreatedAt = DateTime.Now
                };

                _context.Add(reservation);
                await _context.SaveChangesAsync();

                try
                {
                    if (user.Email != null && court != null)
                    {
                        await _emailService.SendReservationConfirmationAsync(user.Email, reservation, court);
                    }
                }
                catch
                {
                }

                TempData["SuccessMessage"] = "Rezerwacja została utworzona pomyślnie.";
                return RedirectToAction(nameof(Index));
            }

            var activeCourts = await _context.Courts
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.CourtId = new SelectList(activeCourts, "Id", "Name", reservationDTO.CourtId);
            ViewBag.SelectedDate = reservationDTO.Date;

            return View(reservationDTO);
        }

        // POST: Reservations/Cancel/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetUserId();

            var reservation = await _context.Reservations
                .Include(r => r.Court)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            if (reservation.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (reservation.Status == "Cancelled")
            {
                TempData["ErrorMessage"] = "Rezerwacja została już anulowana.";
                return RedirectToAction(nameof(Index));
            }

            reservation.Status = "Cancelled";
            reservation.CancelledAt = DateTime.Now;

            _context.Update(reservation);
            await _context.SaveChangesAsync();

            try
            {
                if (reservation.User.Email != null && reservation.Court != null)
                {
                    await _emailService.SendReservationCancellationAsync(
                        reservation.User.Email, 
                        reservation, 
                        reservation.Court);
                }
            }
            catch
            {
            }

            TempData["SuccessMessage"] = "Rezerwacja została anulowana pomyślnie.";
            return RedirectToAction(nameof(Index));
        }

        private static bool TryParseTime(string timeStr, out TimeOnly timeOnly)
        {
            var parts = timeStr.Split(':');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int hours) &&
                int.TryParse(parts[1], out int minutes) &&
                hours >= 0 && hours < 24 &&
                minutes >= 0 && minutes < 60)
            {
                timeOnly = new TimeOnly(hours, minutes, 0);
                return true;
            }
            timeOnly = TimeOnly.MinValue;
            return false;
        }
    }
}

