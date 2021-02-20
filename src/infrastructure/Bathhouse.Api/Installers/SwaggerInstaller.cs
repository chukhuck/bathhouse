using Bathhouse.Api.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Bathhouse.Api.Installers
{
  public class SwaggerInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {
      var identityServiceOpt = new IdentityServiceOpt();
      Configuration.Bind("IdentityService", identityServiceOpt);

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
          new OpenApiInfo
          {
            Title = "Bathhouse.Api",
            Version = "v1",
            Description = "An API to Bathhouse",
            Contact = new OpenApiContact
            {
              Name = "Alex Potapchuk",
              Email = "a.v.potapchuk@yandex.ru"
            },
            License = new OpenApiLicense
            {
              Name = "Apache 2.0",
              Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html")
            }
          });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        // Set the comments path for the Swagger JSON and UI.
        var xmlFileDTO = $"Bathhouse.xml";
        var xmlPathDTO = Path.Combine(AppContext.BaseDirectory, xmlFileDTO);
        c.IncludeXmlComments(xmlPathDTO);    

        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
          Type = SecuritySchemeType.OAuth2,
          Flows = new OpenApiOAuthFlows
          {
            Password= new OpenApiOAuthFlow 
            {
              AuthorizationUrl = new Uri(identityServiceOpt.AuthUrl),
              TokenUrl = new Uri(identityServiceOpt.TokenUrl),
              Scopes = new Dictionary<string, string>
              {
                  {"bathhouse", "Demo API - full access"}
              }
            },
          }
        });

        c.OperationFilter<AuthorizeCheckOperationFilter>();
      });
    }
  }

  public class AuthorizeCheckOperationFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      var hasAuthorize =
        context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
        || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

      if (hasAuthorize)
      {
        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

        operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme {Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"}
                        }
                    ] = new[] {"bathhouse"}
                }
            };

      }
    }
  }
}
