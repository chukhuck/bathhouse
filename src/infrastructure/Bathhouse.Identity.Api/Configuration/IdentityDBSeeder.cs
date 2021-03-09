using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Bathhouse.Identity.Api.Configuration
{
  public static class IdentityDBSeeder
  {
    public static void SeedIdentityDatabase(this IApplicationBuilder app, IConfiguration configuration)
    {
#pragma warning disable IDE0063
      using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        context.Database.Migrate();

        var _identityConfig = new Config(configuration);


        if (!context.Clients.Any())
        {
          foreach (var client in _identityConfig.GetClients())
          {
            context.Clients.Add(client.ToEntity());
          }
          context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
          foreach (var resource in _identityConfig.GetResources())
          {
            context.IdentityResources.Add(resource.ToEntity());
          }
          context.SaveChanges();
        }

        if (!context.ApiResources.Any())
        {
          foreach (var resource in _identityConfig.GetApis())
          {
            context.ApiResources.Add(resource.ToEntity());
          }
          context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
          foreach (var resource in _identityConfig.GetApiScopes())
          {
            context.ApiScopes.Add(resource.ToEntity());
          }
          context.SaveChanges();
        }
      }
    }
  }
}
