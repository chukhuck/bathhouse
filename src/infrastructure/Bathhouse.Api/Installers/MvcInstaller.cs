using Bathhouse.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Bathhouse.Api.Installers
{
  public class MvcInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {

      services.AddControllers()
        .AddJsonOptions(options =>
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        
      })
        .AddIdentityServerAuthentication(options =>
        {
          options.Authority = Configuration.GetValue<string>("IdentityService:URL");
          options.SaveToken = true;
          options.RequireHttpsMetadata = false;
          options.ApiSecret = Configuration.GetValue<string>("IdentityService:Secret");
          options.ApiName = "bathhouse";
        });

      services.AddAuthorization(option =>
      {
        option.AddPolicy(Constants.AdminRoleName, 
          adminPolicy => adminPolicy.RequireClaim("role", Constants.AdminRoleName));

        option.AddPolicy(Constants.DirectorRoleName, 
          directorPolicy => directorPolicy.RequireClaim("role", Constants.DirectorRoleName));

        option.AddPolicy(Constants.ManagerRoleName,
          managerPolicy => managerPolicy.RequireClaim("role", Constants.ManagerRoleName));

        option.AddPolicy(Constants.OfficeModifyPolicy,
          officeModifyPolicy => officeModifyPolicy.RequireClaim(
            "role", Constants.AdminRoleName, Constants.DirectorRoleName));

        option.AddPolicy(Constants.EmployeeAddOrDeletePolicy,
          employeeAddOrDeletePolicy => employeeAddOrDeletePolicy.RequireClaim(
            "role", Constants.AdminRoleName, Constants.DirectorRoleName));
      });


      services.AddCors();
    }
  }
}
