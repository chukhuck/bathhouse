using Bathhouse.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Bathhouse.Memory.Repositories;
using AutoMapper;
using System;
using Microsoft.AspNetCore.Identity;

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


      services.AddSingleton<IRepository<Office>, MemoryBaseCRUDRepository<Office>>();
      services.AddSingleton<IRepository<Employee>, MemoryBaseCRUDRepository<Employee>>();
      services.AddSingleton<IRepository<Client>, MemoryBaseCRUDRepository<Client>>();
      services.AddSingleton<IRepository<WorkItem>, MemoryBaseCRUDRepository<WorkItem>>();
      services.AddSingleton<IRepository<Survey>, MemoryBaseCRUDRepository<Survey>>();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
  }
}
