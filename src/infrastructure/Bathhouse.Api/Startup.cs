using Chuk.Helpers.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bathhouse.Api
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
      var currentAssembly = typeof(Startup).Assembly;
      services.InstallServicesInAssembly(currentAssembly, Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app, 
      IWebHostEnvironment env, 
      IApiVersionDescriptionProvider provider)
    {
      app.UseExceptionHandler("/error");

      if (env.IsDevelopment())
      {
        app.UseExceptionHandler("/error-local-development");
      }

      app.UseHttpsRedirection();


      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.DisplayOperationId();
        c.OAuthClientId("bathhouseswaggerui");

        foreach (var description in provider.ApiVersionDescriptions)
        {
          c.SwaggerEndpoint(
                  $"/swagger/{description.GroupName}/swagger.json",
                  "Bathhouse.Api " + description.GroupName.ToUpperInvariant());
        }
      });

      app.UseHealthChecks("/selfcheck", new HealthCheckOptions
      {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
      }).UseHealthChecksUI(setup =>
      {
        setup.AddCustomStylesheet($"{env.ContentRootPath}/HealthChecks/Ux/branding.css");
      });

      app.UseRouting();

      app.UseCors(policy =>
        policy
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader());

      app.UseAuthentication();
      app.UseAuthorization();


      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
