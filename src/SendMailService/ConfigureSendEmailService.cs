using Microsoft.Extensions.DependencyInjection;

namespace SendMailService
{
    public static class ConfigureSendEmailService
    {
        public static void AddSendEmailService(this IServiceCollection services) =>
            services.AddTransient<ISendEmailService, SendEmailService>();
    }
}