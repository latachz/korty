using Korty.Data;
using Korty.DTOs;
using Korty.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Korty.Controllers
{
    public class CourtsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourtsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var courts = await _context.Courts 
                .OrderBy(c => c.Name)
                .ToListAsync();
            
            var courtDTOs = courts.Select(c => new CourtDTO(c)).ToList();
            return View(courtDTOs);
        }

        // GET: Courts/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id, DateTime? date)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _context.Courts
                .Include(c => c.Reservations)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (court == null)
            {
                return NotFound();
            }

            var checkDate = date ?? DateTime.Today;

            var courtDTO = new CourtDTO(court);
            
            ViewBag.SelectedDate = checkDate;

            return View(courtDTO);
        }

        // GET: Courts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Description,IsActive,OpeningHour,ClosingHour")] Court court)
        {
            if (ModelState.IsValid)
            {
                _context.Add(court);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Kort został dodany pomyślnie.";
                return RedirectToAction(nameof(Index));
            }
            return View(court);
        }

        // GET: Courts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _context.Courts.FindAsync(id);
            if (court == null)
            {
                return NotFound();
            }
            return View(court);
        }

        // POST: Courts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsActive,OpeningHour,ClosingHour")] Court court)
        {
            if (id != court.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(court);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Kort został zaktualizowany pomyślnie.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourtExists(court.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(court);
        }

        // GET: Courts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _context.Courts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (court == null)
            {
                return NotFound();
            }

            return View(court);
        }

        // POST: Courts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var court = await _context.Courts.FindAsync(id);
            if (court != null)
            {
                _context.Courts.Remove(court);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Kort został usunięty pomyślnie.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourtExists(int id)
        {
            return _context.Courts.Any(e => e.Id == id);
        }
    }
}

