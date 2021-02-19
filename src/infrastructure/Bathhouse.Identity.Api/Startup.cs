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
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

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
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bathhouse.Identity.Api", Version = "v1" });
      });

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
      .AddInMemoryIdentityResources(_identityInMemoryConfig.GetResources());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(
          c =>
          {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bathhouse.Identity.Api v1");
            c.OAuthClientId("bathhouseswaggerui");
            c.OAuthAppName("Bathhouse Swagger UI");
          });
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

  //public class SecurityRequirementsDocumentFilter : IDocumentFilter
  //{
  //  public void Apply(SwaggerDocument document, DocumentFilterContext context)
  //  {
  //    document.Security = new List<IDictionary<string, IEnumerable<string>>>()
  //      {
  //          new Dictionary<string, IEnumerable<string>>()
  //          {
  //              { "Bearer", new string[]{ } },
  //              { "Basic", new string[]{ } },
  //              { "oauth2", new string[]{ } },
  //          }
  //      };
  //  }
  //}
}
