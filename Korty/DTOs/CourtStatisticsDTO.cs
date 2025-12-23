namespace Korty.DTOs
{
    public class CourtStatisticsDTO
    {
        public int CourtId { get; set; }
        public string CourtName { get; set; } = string.Empty;
        public int TotalReservations { get; set; }
        public int ActiveReservations { get; set; }
        public int CancelledReservations { get; set; }
        public double TotalHours { get; set; }
    }
}

