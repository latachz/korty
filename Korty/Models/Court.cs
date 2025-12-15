using System.ComponentModel.DataAnnotations;

namespace Korty.Models
{
    public class Court
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nazwa kortu jest wymagana")]
        [Display(Name = "Nazwa kortu")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Display(Name = "Opis")]
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Display(Name = "Aktywny")]
        public bool IsActive { get; set; } = true;
        
        [Display(Name = "Godzina otwarcia")]
        [DataType(DataType.Time)]
        public TimeOnly OpeningHour { get; set; } = new TimeOnly(6, 0, 0);
        
        [Display(Name = "Godzina zamknięcia")]
        [DataType(DataType.Time)]
        public TimeOnly ClosingHour { get; set; } = new TimeOnly(23, 0, 0);
        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}

