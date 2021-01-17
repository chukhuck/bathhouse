using Bathhouse.Api.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Bathhouse.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      IHost host = CreateHostBuilder(args).Build();

      if (args.Length == 1 && args[0] == "--swagger")
      {
        var json = host.GenerateSwagger("v1", null);
        File.WriteAllText("swagger.json", json);
        System.Console.WriteLine($"File swagger.json generated successfully.");
      }
      else
      {
        host.Run();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
