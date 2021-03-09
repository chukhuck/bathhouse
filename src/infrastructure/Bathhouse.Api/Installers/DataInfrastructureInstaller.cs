﻿using Bathhouse.Api.Common.Mapper.v1;
using Bathhouse.EF.Data;
using Bathhouse.EF.Repositories.Common;
using Bathhouse.Entities;
using Bathhouse.Repositories.Common;
using Chuk.Helpers.AspNetCore;
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
      bool useInMemoryStorage = Configuration.GetValue<bool>("UseInMemoryStorage");

      services.AddDbContext<BathhouseContext>(options =>
      {
        if (useInMemoryStorage)
        {
          options
          .UseInMemoryDatabase("BathhouseInMemoryDB")
          .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug)))
          .EnableSensitiveDataLogging();
        }
        else
        {
          options.UseSqlServer(
            Configuration.GetConnectionString("BathhouseDB"),
            sqlOptions => sqlOptions.EnableRetryOnFailure(5));
        }
      });

      services.AddIdentity<Employee, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BathhouseContext>();

      services.AddScoped<IBathhouseUnitOfWork, BathhouseUnitOfWork>();

      services.AddAutoMapper(typeof(EntityMappingProfile));
    }
  }
}
