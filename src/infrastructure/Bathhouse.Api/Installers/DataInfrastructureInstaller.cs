using Bathhouse.Contracts.Infrastructure;
using Bathhouse.EF.Data;
using Bathhouse.EF.Repositories;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bathhouse.Api.Installers
{
  public class DataInfrastructureInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {
      bool useTestDataInMemory = Configuration.GetValue<bool>("UseTestDataInMemory");

      services.AddDbContext<BathhouseContext>(options =>
      {
        if (useTestDataInMemory)
        {
          options
          .UseInMemoryDatabase("BathhouseInMemoryDB")
          .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug)))
          .EnableSensitiveDataLogging();
        }
        else
        {
          options.UseSqlServer(Configuration.GetConnectionString("BathhouseDB"));
        }
      });

      services.AddIdentity<Employee, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BathhouseContext>();

      services.AddScoped<IUnitOfWork, UnitOfWork>();

      services.AddAutoMapper(typeof(EntityMappingProfile));
    }
  }
}
