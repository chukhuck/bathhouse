using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Bathhouse.Api.Extensions
{
  public static class SwaggerSpecHostExtensions
  {
    public static string GenerateSwagger(this IHost host, string docName, string? basePath)
    {
      try
      {
        var sw = host.Services.GetRequiredService<ISwaggerProvider>();
        var doc = sw.GetSwagger(docName, null, basePath);

        using var streamWriter = new StringWriter();

        var writer = new OpenApiJsonWriter(streamWriter);
        doc.SerializeAsV3(writer);
        return streamWriter.ToString();
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static void GenerateSwaggerSpecification(this IHost host, ILogger logger)
    {
      try
      {
        var provider = host.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
          var json = host.GenerateSwagger(description.GroupName, null);
          File.WriteAllText(@$"..\..\..\docs\spec\{description.GroupName}\Bathhouse.json", json);
          logger.LogInformation($"File Bathhouse.{description.GroupName}.json generated successfully.");
        }
      }
      catch (Exception ex)
      {
        logger.LogError(ex, $"Error in {nameof(GenerateSwaggerSpecification)}");
      }
    }
  }
}
