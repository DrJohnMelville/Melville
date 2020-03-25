using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace SendMailService
{
  public sealed class SendEmailService:ISendEmailService
  {
    // config Data
    public string SourceAccount { get; set; } = "";
    public string SourceName { get; set; } = "CapWeb Account Robot";
    public string SmtpServer { get; set; } = "";
    public string Password { get; set; } = "";
    public int SmtpPort { get; set; }

    private readonly ILogger log;
    
    public SendEmailService(ILogger<SendEmailService> log, IConfiguration configuration)
     {
      this.log = log;
      configuration.GetSection("Email").Bind(this);
     }

    public async Task SendEmail(string emailAddress, string subject, string htmlBody)
    {
      // retry failed sending up to 5 times, with an increasing delay.
      for (int i = 0; i < 5; i++)
      {
        try
        {
          await DoSend(emailAddress, subject, htmlBody);
          return;
        }
        catch (Exception )
        {
          if (i == 4) throw;  /// preserve the final exception
        }
        await Task.Delay(TimeSpan.FromSeconds(1 + (2 * i)));
      }
    }

    private Task DoSend(string emailAddress, string subject, string htmlBody)
    {
      log.Log(LogLevel.Information, $"Email sent to {emailAddress}: Subject");
      log.Log(LogLevel.Trace, $"Message Text: {htmlBody}");
      return Send(CreateMimeMessage(emailAddress, subject, htmlBody));
    }

    private async Task Send(MimeMessage mimeMessage)
    {
      using var client = new SmtpClient
      {
        ServerCertificateValidationCallback = AcceptAnySSLCertificate,
        CheckCertificateRevocation = false
      };
      await client.ConnectAsync(SmtpServer, SmtpPort, true);
      await client.AuthenticateAsync(SourceAccount, Password);
      await client.SendAsync(mimeMessage);
      await client.DisconnectAsync(true);
    }

    private bool AcceptAnySSLCertificate(object s, X509Certificate c, X509Chain h, SslPolicyErrors e) 
      => true;

    private MimeMessage CreateMimeMessage(string email, string subject, string htmlMessage)
    {
      var mimeMessage = new MimeMessage();
      mimeMessage.From.Add(new MailboxAddress(SourceName, SourceAccount));
      mimeMessage.To.Add(new MailboxAddress(email));
      mimeMessage.Subject = subject;
      mimeMessage.Body = new TextPart("html"){Text = htmlMessage};
      return mimeMessage;
    }
  }
}