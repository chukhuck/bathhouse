using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bathhouse.Api.Extensions
{
  public static class IHostExtensions
  {
    public static string GenerateSwagger(this IHost host, string docName, string? basePath)
    {
      var sw = host.Services.GetRequiredService<ISwaggerProvider>();
      var doc = sw.GetSwagger(docName, null, basePath);

      using (var streamWriter = new StringWriter())
      {
        var writer = new OpenApiJsonWriter(streamWriter);
        doc.SerializeAsV3(writer);
        return streamWriter.ToString();
      }
    }
  }
}
