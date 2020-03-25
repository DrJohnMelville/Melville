using System.Threading.Tasks;

namespace SendMailService
{
    public interface ISendEmailService
    {
        Task SendEmail(string emailAddress, string subject, string htmlBody);
    }
}