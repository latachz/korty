using Korty.Models;
using System.Net;

namespace Korty.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendReservationConfirmationAsync(string email, Reservation reservation, Court court)
        {
            try
            {
                var subject = "Potwierdzenie rezerwacji kortu";
                var body = $@"
Witaj,

Twoja rezerwacja kortu została potwierdzona.

Szczegóły rezerwacji:
- Kort: {court.Name}
- Data: {reservation.Date:dd.MM.yyyy}
- Godzina: {reservation.StartTime:HH\\:mm} - {reservation.EndTime:HH\\:mm}
- Status: {reservation.Status}

Dziękujemy za skorzystanie z naszych usług.

Pozdrawiamy,
Zespół Korty
";

                await SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas wysyłania emaila z potwierdzeniem rezerwacji");
                throw;
            }
        }

        public async Task SendReservationCancellationAsync(string email, Reservation reservation, Court court)
        {
            try
            {
                var subject = "Anulowanie rezerwacji kortu";
                var body = $@"
Witaj,

Twoja rezerwacja kortu została anulowana.

Szczegóły anulowanej rezerwacji:
- Kort: {court.Name}
- Data: {reservation.Date:dd.MM.yyyy}
- Godzina: {reservation.StartTime:hh\\:mm} - {reservation.EndTime:hh\\:mm}
- Data anulowania: {reservation.CancelledAt:dd.MM.yyyy HH:mm}

Jeśli masz pytania, skontaktuj się z nami.

Pozdrawiamy,
Zespół Korty
";

                await SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas wysyłania emaila o anulowaniu rezerwacji");
                throw;
            }
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {          
            // SMTP implementation not configured
            // SmtpClient client = new SmtpClient(server, port)
            // client.Send(new MailMessage(fromEmail, toEmail, subject, body))

            _logger.LogInformation("Email wyłączony. Treść wiadomości:\nTo: {To}\nSubject: {Subject}\nBody: {Body}", 
                    toEmail, subject, body);
            return;

        }
    }
}

