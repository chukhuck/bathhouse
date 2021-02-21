using Bathhouse.EF.Data;
using Bathhouse.Entities;
using Bathhouse.Identity.Api.Certificates;
using Bathhouse.Identity.Api.Configuration;
using Bathhouse.Identity.Api.Services;
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
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      Environment = env;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

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

      var identityServerBuilder = services.AddIdentityServer(x =>
      {
        x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
        
      })
      .AddAspNetIdentity<Employee>()
      .AddInMemoryApiResources(_identityInMemoryConfig.GetApis())
      .AddInMemoryClients(_identityInMemoryConfig.GetClients())
      .AddInMemoryIdentityResources(_identityInMemoryConfig.GetResources())
      .AddInMemoryApiScopes(_identityInMemoryConfig.GetApiScopes())
      .AddProfileService<ProfileService>();

      if (Environment.IsDevelopment())
      {
        identityServerBuilder.AddDeveloperSigningCredential();
      }
      else
      {
        identityServerBuilder.AddCertificateFromFile(Configuration);
      }

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
