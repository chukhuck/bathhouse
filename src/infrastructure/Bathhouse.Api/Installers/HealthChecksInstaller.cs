using Bathhouse.EF.Data;
using Chuk.Helpers.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace Bathhouse.Api.Installers
{
  public class HealthChecksInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {
      services.AddHealthChecks()
         .AddPingHealthCheck(
                    option => option.AddHost("www.yandex.com", 600000),
                    name: "Yandex Ping")
         .AddIdentityServer(
                     idSvrUri: new Uri(Configuration["ApiResourceBaseUrls:AuthServer"]),
                     name: "Auth Server",
                     failureStatus: HealthStatus.Degraded)
         .AddDbContextCheck<BathhouseContext>(
                     name: "BathhouseContext",
                     failureStatus: HealthStatus.Degraded)
         .AddSqlServer(
                     connectionString: Configuration["ConnectionStrings:BathhouseDB"],
                     healthQuery: "SELECT 1;",
                     name: "SQL",
                     failureStatus: HealthStatus.Degraded,
                     tags: new string[] { "db", "sql", "sqlserver" });

      services
        .AddHealthChecksUI()
        .AddSqlServerStorage(Configuration["ConnectionStrings:HealthCheckDB"]);
    }
  }
}
