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
using System.Reflection;

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
        options.UseSqlServer(
          Configuration.GetConnectionString("BathhouseDB"),
          sqlOptions => sqlOptions.EnableRetryOnFailure(5));
      });

      services.AddIdentity<Employee, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BathhouseContext>()
                .AddDefaultTokenProviders();

      var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
      var _identityInMemoryConfig = new Config(Configuration);

      var identityServerBuilder = services
        .AddIdentityServer(x =>
          x.Authentication.CookieLifetime = TimeSpan.FromHours(2))
        .AddAspNetIdentity<Employee>()
        .AddProfileService<ProfileService>()
        .AddConfigurationStore(options =>
          {
            options.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("IdentityServerDB"),
              sqlOptions =>
              {
                sqlOptions.MigrationsAssembly(migrationsAssembly);
                sqlOptions.EnableRetryOnFailure(5);
              });
          })
        .AddOperationalStore(options =>
          {
            options.ConfigureDbContext = b => b.UseSqlServer(Configuration.GetConnectionString("IdentityServerDB"),
                sqlOptions =>
                {
                  sqlOptions.MigrationsAssembly(migrationsAssembly);
                  sqlOptions.EnableRetryOnFailure(5);
                });
          }); ;

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
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
      app.SeedIdentityDatabase(configuration);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors(policy =>
        policy
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader());

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
