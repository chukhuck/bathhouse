using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Bathhouse.Api.Installers
{
  public class SwaggerInstaller : IServiceInstaller
  {
    public void InstallService(IServiceCollection services, IConfiguration Configuration)
    {
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

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
          Name = "Authorization",
          Description = "JWT Authorization header using the bearer scheme",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey

        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
           {
             new OpenApiSecurityScheme
               {
                   Reference = new OpenApiReference
                   {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                   }
               },
                   Array.Empty<string>()
           }
          });

      });
    }
  }
}
