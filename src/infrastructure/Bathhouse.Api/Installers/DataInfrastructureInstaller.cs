using Bathhouse.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Bathhouse.Entities;
using Bathhouse.Repositories;
using Bathhouse.Memory.Repositories;
using AutoMapper;
using System;

namespace Bathhouse.Api.Installers
{
  public class DataInfrastructureInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {
      services.AddDbContext<BathhouseContext>(options =>
      options.UseSqlServer(
        Configuration.GetConnectionString("BathhouseDB")));

      services.AddIdentity<Employee, Role>()
              .AddEntityFrameworkStores<BathhouseContext>();


      services.AddSingleton<ICRUDRepository<Office>, MemoryBaseCRUDRepository<Office>>();
      services.AddSingleton<ICRUDRepository<Employee>, MemoryBaseCRUDRepository<Employee>>();
      services.AddSingleton<ICRUDRepository<Client>, MemoryBaseCRUDRepository<Client>>();
      services.AddSingleton<ICRUDRepository<WorkItem>, MemoryBaseCRUDRepository<WorkItem>>();
      services.AddSingleton<ICRUDRepository<Survey>, MemoryBaseCRUDRepository<Survey>>();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
  }
}
