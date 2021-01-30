using Bathhouse.Api.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Bathhouse.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      IHost host = CreateHostBuilder(args).Build();

      var serviceProvider = host.Services;
      var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger(typeof(Program));

      if (args.Length > 0 && args.Contains("--swagger"))
      {
        var json = host.GenerateSwagger("v1", null);
        File.WriteAllText("swagger.json", json);
        logger.LogInformation($"File swagger.json generated successfully.");
      }

      if (args.Length > 0 && args.Contains("--seed"))
      {
        logger.LogInformation("Seeding");
      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
