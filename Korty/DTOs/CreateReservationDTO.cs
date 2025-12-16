using System.ComponentModel.DataAnnotations;

namespace Korty.DTOs
{
    public class CreateReservationDTO
    {
        [Required(ErrorMessage = "Kort jest wymagany")]
        [Display(Name = "Kort")]
        public int CourtId { get; set; }
        
        [Required(ErrorMessage = "Data jest wymagana")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Godzina rozpoczęcia jest wymagana")]
        [Display(Name = "Godzina rozpoczęcia")]
        [DataType(DataType.Time)]
        public string StartTime { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Godzina zakończenia jest wymagana")]
        [Display(Name = "Godzina zakończenia")]
        [DataType(DataType.Time)]
        public string EndTime { get; set; } = string.Empty;
    }
}

