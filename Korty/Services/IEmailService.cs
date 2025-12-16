using Korty.Models;

namespace Korty.Services
{
    public interface IEmailService
    {
        Task SendReservationConfirmationAsync(string email, Reservation reservation, Court court);
        Task SendReservationCancellationAsync(string email, Reservation reservation, Court court);
    }
}

