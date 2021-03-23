using BettingLine.Service.BackgroundService;
using BettingLine.Service.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BettingLine.Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigurePublisherService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<OutcomesChangesProcessorBackgroundService>();
            
            var serviceClientSettingsConfig = configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            services.AddSingleton<IMessagePublisher, MessagePublisher>();
        }
    }
}