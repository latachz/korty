using System.ComponentModel.DataAnnotations;

namespace Korty.DTOs
{
    public class CourtAvailabilityDTO
    {
        [Display(Name = "Kort")]
        public int CourtId { get; set; }
        
        [Display(Name = "Nazwa kortu")]
        public string CourtName { get; set; } = string.Empty;
        
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Display(Name = "Dostępne przedziały czasowe")]
        public List<TimeRangeDTO> AvailableTimeRanges { get; set; } = new List<TimeRangeDTO>();
        
        [Display(Name = "Zarezerwowane przedziały czasowe")]
        public List<TimeRangeDTO> ReservedTimeRanges { get; set; } = new List<TimeRangeDTO>();
        
        [Display(Name = "Godzina otwarcia")]
        public TimeOnly OpeningHour { get; set; }
        
        [Display(Name = "Godzina zamknięcia")]
        public TimeOnly ClosingHour { get; set; }
    }
    
    public class TimeRangeDTO
    {
        [Display(Name = "Godzina rozpoczęcia")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }
        
        [Display(Name = "Godzina zakończenia")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }
    }
}

