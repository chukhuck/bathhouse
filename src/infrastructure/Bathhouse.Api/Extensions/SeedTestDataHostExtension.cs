using Bathhouse.EF.Data;
using Bathhouse.EF.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Bathhouse.Api.Extensions
{
  public static class SeedTestDataHostExtension
  {
    public static void SeedTestData(this IHost host)
    {
#pragma warning disable IDE0063
      using var scope = host.Services.CreateScope();
      using (BathhouseContext context = scope.ServiceProvider.GetRequiredService<BathhouseContext>())
      {
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        string baseDirectory = Path.GetDirectoryName(typeof(Program)?.Assembly?.Location) ?? string.Empty;

        var builder = new ConfigurationBuilder()
          .SetBasePath(baseDirectory)
          .AddJsonFile("Faker.json");
        var configuration = builder.Build();

        var opt = new DataFakerOption();
        configuration.GetSection(DataFakerOption.Position).Bind(opt);

        DataFaker.Generate(context, opt);
      }
#pragma warning restore IDE0063
    }
  }
}
