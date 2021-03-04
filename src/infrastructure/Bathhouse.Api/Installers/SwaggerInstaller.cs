using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Bathhouse.Api.Installers
{
  public class SwaggerInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {
      services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
      //Use a logic from SwaggerConfigureOptions
      services.AddSwaggerGen(option => Console.WriteLine("Use a logic from SwaggerConfigureOptions"));
    }
  }
}
