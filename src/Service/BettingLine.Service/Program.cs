using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingLine.Service.BackgroundService;
using BettingLine.Service.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BettingLine.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, confBuilder) =>
                {
                    var environment = hostContext.HostingEnvironment.EnvironmentName;
                    confBuilder.AddEnvironmentVariables();
                    confBuilder.AddJsonFile($"appsettings.{environment}.json", true, false);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
