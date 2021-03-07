using Chuk.Helpers.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Bathhouse.Api.Installers
{
  public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
  {
    readonly IApiVersionDescriptionProvider provider;
    readonly IConfiguration Configuration;

    public SwaggerConfigureOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
      this.provider = provider;
      Configuration = configuration;
    }

    public void Configure(SwaggerGenOptions options)
    {
      foreach (var description in provider.ApiVersionDescriptions)
      {
        options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
      }

      IncludeXmlComment(options);
      AddSecurityDefinition(options);
      SetOperationFilters(options);

      options.SupportNonNullableReferenceTypes();
    }

    private static void SetOperationFilters(SwaggerGenOptions options)
    {
      options.OperationFilter<AuthorizeCheckOperationFilter>();
      options.OperationFilter<SwaggerDefaultValuesFilter>();
    }

    private void AddSecurityDefinition(SwaggerGenOptions options)
    {
      options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
      {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
          Password = new OpenApiOAuthFlow
          {
            AuthorizationUrl = new Uri(Configuration.GetValue<string>("ApiResourceBaseUrls:AuthEndpointAuthServer")),
            TokenUrl = new Uri(Configuration.GetValue<string>("ApiResourceBaseUrls:TokenEndpointAuthServer")),
            Scopes = new Dictionary<string, string>
              {
                  {Configuration.GetValue<string>("Self:Id"), "Bathhouse API - full access"}
              }
          },
        }
      });
    }

    private static void IncludeXmlComment(SwaggerGenOptions options)
    {
      // Set the comments path for the Swagger JSON and UI.
      var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
      options.IncludeXmlComments(xmlPath);

      // Set the comments path for the Swagger JSON and UI.
      var xmlFileDTO = $"Bathhouse.xml";
      var xmlPathDTO = Path.Combine(AppContext.BaseDirectory, xmlFileDTO);
      options.IncludeXmlComments(xmlPathDTO);

      // Set the comments path for the Swagger JSON and UI.
      var xmlFileContract = $"Bathhouse.Api.Contracts.xml";
      var xmlPathContract = Path.Combine(AppContext.BaseDirectory, xmlFileContract);
      options.IncludeXmlComments(xmlPathContract);
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
      var info = new OpenApiInfo()
      {
        Title = "Bathhouse.API",
        Version = description.ApiVersion.ToString(),
        Description = "An API to Bathhouse.",
        Contact = new OpenApiContact() { Name = "Alex Potapchuk", Email = "a.v.potapchuk@yandex.ru" },
        License = new OpenApiLicense() { Name = "Apache 2.0", Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html") }
      };

      if (description.IsDeprecated)
      {
        info.Description += " This API version has been deprecated.";
      }

      return info;
    }
  }
}
