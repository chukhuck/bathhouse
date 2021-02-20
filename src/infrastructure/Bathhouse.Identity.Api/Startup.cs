using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Identity.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Bathhouse.Identity.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllers();

      services.AddDbContext<BathhouseContext>(options =>
      {
          options.UseSqlServer(Configuration.GetConnectionString("BathhouseDB"));
      });

      services.AddIdentity<Employee, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BathhouseContext>()
                .AddDefaultTokenProviders();

      var _identityInMemoryConfig = new Config(Configuration);

      services.AddIdentityServer(x =>
      {
        x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
        
      })
      .AddDeveloperSigningCredential()
      .AddAspNetIdentity<Employee>()
      .AddInMemoryApiResources(_identityInMemoryConfig.GetApis())
      .AddInMemoryClients(_identityInMemoryConfig.GetClients())
      .AddInMemoryIdentityResources(_identityInMemoryConfig.GetResources())
      .AddInMemoryApiScopes(_identityInMemoryConfig.GetApiScopes());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseIdentityServer();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
