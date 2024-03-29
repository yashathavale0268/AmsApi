using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AmsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                   // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    webBuilder.UseStartup<Startup>();
                    //webBuilder.ConfigureAppConfiguration(config => config.AddJsonFile($"ocelot.{env}.json"));
                   // webBuilder.ConfigureAppConfiguration(config => config.AddJsonFile($"ocelot.json"));
                });
     
    }
}
