using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chuk.Helpers.AspNetCore
{
  public interface IServiceInstaller
  {
    void InstallService(IServiceCollection services, IConfiguration Configuration);
  }
}