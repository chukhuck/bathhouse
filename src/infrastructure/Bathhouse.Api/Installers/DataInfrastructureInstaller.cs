using Bathhouse.Contracts.Infrastructure;
using Bathhouse.EF.Data;
using Bathhouse.EF.Repositories;
using Bathhouse.EF.Repositories.Common;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Bathhouse.Repositories.Common;
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

      services.AddScoped<IBathhouseUnitOfWork, BathhouseUnitOfWork>();

      services.AddAutoMapper(typeof(EntityMappingProfile));
    }
  }
}
