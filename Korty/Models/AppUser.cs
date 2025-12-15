using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korty.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
