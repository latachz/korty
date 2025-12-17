using Korty.Models;
using System.ComponentModel.DataAnnotations;

namespace Korty.DTOs
{
    public class CourtDTO
    {
        public int Id { get; set; }
        
        [Display(Name = "Nazwa kortu")]
        public string Name { get; set; } = string.Empty;
        
        [Display(Name = "Opis")]
        public string? Description { get; set; }
        
        [Display(Name = "Aktywny")]
        public bool IsActive { get; set; }
        
        [Display(Name = "Godzina otwarcia")]
        [DataType(DataType.Time)]
        public TimeOnly OpeningHour { get; set; }
        
        [Display(Name = "Godzina zamknięcia")]
        [DataType(DataType.Time)]
        public TimeOnly ClosingHour { get; set; }

        [Display(Name = "Rezerwacje")]
        public List<ReservationDTO> Reservations { get; set; } = new List<ReservationDTO>();
        
        public CourtDTO() { }
        
        public CourtDTO(Court court)
        {
            Id = court.Id;
            Name = court.Name;
            Description = court.Description;
            IsActive = court.IsActive;
            OpeningHour = court.OpeningHour;
            ClosingHour = court.ClosingHour;
            Reservations = court.Reservations.Select(r => new ReservationDTO(r)).ToList();
        }
    }
}

