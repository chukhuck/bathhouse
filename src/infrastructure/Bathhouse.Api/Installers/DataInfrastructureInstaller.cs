using Bathhouse.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Identity;
using Bathhouse.EF.Repositories;

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
          options.UseInMemoryDatabase("BathhouseInMemoryDB");
        }
        else
        {
          options.UseSqlServer(Configuration.GetConnectionString("BathhouseDB"));
        }
      });

      services.AddIdentity<Employee, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BathhouseContext>();

      services.AddScoped<IUnitOfWork, UnitOfWork>();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
  }
}
