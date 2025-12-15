using Korty.Data;
using Korty.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Korty.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public RolesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _context.Users;
            if (users.Count() == 1)
            {
                var me = await _userManager.FindByIdAsync(GetUserId());

                await _userManager.AddToRoleAsync(me, "Admin");
            } else
            {
                Console.WriteLine(users.Count());
            }
            return View();
        }
    }
}
