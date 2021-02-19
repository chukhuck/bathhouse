using Bathhouse.Api.Options;
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

      var jwtOption = new JWTOption();
      Configuration.Bind(nameof(jwtOption), jwtOption);
      services.AddSingleton(jwtOption);

      //services.AddAuthentication(x =>
      //{
      //  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      //  x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      //  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      //})
      //  .AddJwtBearer(options =>
      //  {
      //    options.SaveToken = true;
      //    options.TokenValidationParameters = new TokenValidationParameters()
      //    {
      //      ValidateIssuerSigningKey = true,
      //      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOption.Secret)),
      //      ValidateIssuer = false,
      //      ValidateAudience = false,
      //      RequireExpirationTime = false,
      //      ValidateLifetime = true
      //    };
      //  });

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
        .AddIdentityServerAuthentication(options =>
        {
          options.Authority = Configuration.GetValue<string>("IdentityService:URL");
          options.RequireHttpsMetadata = false;
          options.ApiSecret = Configuration.GetValue<string>("IdentityService:Secret");
          options.ApiName = "bathhouse";
        });

      services.AddCors();
    }
  }
}
