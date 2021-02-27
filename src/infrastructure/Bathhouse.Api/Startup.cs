using Bathhouse.Api.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
      services.InstallServicesInAssembly(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseExceptionHandler("/error");

      if (env.IsDevelopment())
      {
        app.UseExceptionHandler("/error-local-development");
      }

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.DisplayOperationId();
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bathhouse.Api v1");
        c.OAuthClientId("bathhouseswaggerui");
      });


      app.UseHttpsRedirection();

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
