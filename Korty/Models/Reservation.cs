using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korty.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Kort")]
        public int CourtId { get; set; }
        
        [ForeignKey("CourtId")]
        public Court Court { get; set; } = null!;
        
        [Required]
        [Display(Name = "Użytkownik")]
        public string UserId { get; set; } = string.Empty;
        
        [ForeignKey("UserId")]
        public AppUser User { get; set; } = null!;
        
        [Required(ErrorMessage = "Data jest wymagana")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Godzina rozpoczęcia jest wymagana")]
        [Display(Name = "Godzina rozpoczęcia")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }
        
        [Required(ErrorMessage = "Godzina zakończenia jest wymagana")]
        [Display(Name = "Godzina zakończenia")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
        
        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Active";
        
        [Display(Name = "Data utworzenia")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [Display(Name = "Data anulowania")]
        public DateTime? CancelledAt { get; set; }
    }
}

