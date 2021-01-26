using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bathhouse.Api.Installers
{
  public interface IServiceInstaller
  {
    void InstallService(IServiceCollection services, IConfiguration Configuration);
  }
}