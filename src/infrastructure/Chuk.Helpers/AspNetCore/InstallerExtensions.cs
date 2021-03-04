using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Chuk.Helpers.AspNetCore
{
  public static class InstallerExtensions
  {
    public static void InstallServicesInAssembly(this IServiceCollection services, Assembly assembly, IConfiguration configuration)
    {
      var installers = assembly.ExportedTypes.
                                  Where(x => typeof(IServiceInstaller).IsAssignableFrom(x) &&
                                            !x.IsInterface &&
                                            !x.IsAbstract).
                                  Select(Activator.CreateInstance).
                                  Cast<IServiceInstaller>().ToList();

      installers.ForEach(i => i.InstallService(services, configuration));
    }
  }
}
