using Bathhouse.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Bathhouse.EF.InMemory
{
  public class BathhouseDbFixture
  {
#pragma warning disable CA1822
    private static readonly object _lock = new object();
    private static bool _databaseInitialized;

    public BathhouseDbFixture()
    {
      Seed();
    }

    public BathhouseContext CreateContext()
    {
      var ctx = new BathhouseContext(
        new DbContextOptionsBuilder<BathhouseContext>()
        .UseLazyLoadingProxies()
        .UseInMemoryDatabase("BathHouseDbTest")
        .Options);

      return ctx;
    }

    private void Seed()
    {
      lock (_lock)
      {
        if (!_databaseInitialized)
        {
          using (var context = CreateContext())
          {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("Faker.json");
            var configuration = builder.Build();

            var opt = new DataFakerOption();
            configuration.GetSection(DataFakerOption.Position).Bind(opt);

            DataFaker.Generate(context, opt);

            context.SaveChanges();
          }

          _databaseInitialized = true;
        }
      }
    }
  }
}
