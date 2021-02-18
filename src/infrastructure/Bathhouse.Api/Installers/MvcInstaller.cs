using Bathhouse.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

      var jwtOption = new JWTOption();
      Configuration.Bind(nameof(jwtOption), jwtOption);
      services.AddSingleton(jwtOption);

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
        .AddJwtBearer(options =>
        {
          options.SaveToken = true;
          options.TokenValidationParameters = new TokenValidationParameters()
          {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOption.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
          };
        });

      services.AddCors();
    }
  }
}
