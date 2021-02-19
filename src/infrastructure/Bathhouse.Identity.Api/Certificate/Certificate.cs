using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Bathhouse.Identity.Api.Certificates
{
  static class Certificate
  {
    public static IIdentityServerBuilder AddCertificateFromFile(
    this IIdentityServerBuilder builder,
    IConfiguration config)
    {
      var keyFilePath = config["Certificate:Path"];
      var keyFilePassword = config["Certificate:Password"];
      if (File.Exists(keyFilePath))
      {
        builder.AddSigningCredential(
          new X509Certificate2(keyFilePath, keyFilePassword));
      }

      return builder;
    }
  }
}
