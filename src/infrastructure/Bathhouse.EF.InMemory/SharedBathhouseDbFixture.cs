using Bathhouse.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bathhouse.EF.InMemory
{
  public class SharedBathhouseDbFixture
  {
#pragma warning disable CA1822
    private static readonly object _lock = new object();
    private static bool _databaseInitialized;

    public SharedBathhouseDbFixture()
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
