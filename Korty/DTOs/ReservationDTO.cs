using Korty.Models;
using System.ComponentModel.DataAnnotations;

namespace Korty.DTOs
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        
        [Display(Name = "Kort")]
        public int CourtId { get; set; }
        
        [Display(Name = "Nazwa kortu")]
        public string CourtName { get; set; } = string.Empty;
        
        [Display(Name = "ID użytkownika")]
        public string UserId { get; set; } = string.Empty;
        
        [Display(Name = "Email użytkownika")]
        public string UserEmail { get; set; } = string.Empty;

        [Display(Name = "Imię użytkownika")]
        public string UserFirstName { get; set; } = string.Empty;

        [Display(Name = "Nazwisko użytkownika")]
        public string UserLastName { get; set; } = string.Empty;
        
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Display(Name = "Godzina rozpoczęcia")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }
        
        [Display(Name = "Godzina zakończenia")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
        
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;
        
        [Display(Name = "Data utworzenia")]
        public DateTime CreatedAt { get; set; }
        
        [Display(Name = "Data anulowania")]
        public DateTime? CancelledAt { get; set; }
        
        public ReservationDTO() { }
        
        public ReservationDTO(Reservation reservation)
        {
            Id = reservation.Id;
            CourtId = reservation.CourtId;
            CourtName = reservation.Court?.Name ?? string.Empty;
            UserId = reservation.UserId;
            UserEmail = reservation.User?.Email ?? string.Empty;
            UserFirstName = reservation.User?.FirstName ?? string.Empty;
            UserLastName = reservation.User?.LastName ?? string.Empty;
            Date = reservation.Date;
            StartTime = reservation.StartTime;
            EndTime = reservation.EndTime;
            Status = reservation.Status;
            CreatedAt = reservation.CreatedAt;
            CancelledAt = reservation.CancelledAt;
        }
    }
}

