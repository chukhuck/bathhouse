using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bathhouse.Api.Installers
{
  public class VersioningInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {
      services.AddApiVersioning(option =>
      {
        option.AssumeDefaultVersionWhenUnspecified = true;
        option.ReportApiVersions = true;

        option.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        option.ApiVersionSelector = new LowestImplementedApiVersionSelector(option);
      });

      services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'v'VVV");
    }
  }
}
