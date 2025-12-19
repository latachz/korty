using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korty.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;
        
        [PersonalData]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;
        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
