using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Bathhouse.Identity.Api.Configuration
{
  public class Config
  {
    IConfiguration _config;

    public Config(IConfiguration config)
    {
      _config = config;
    }

    // ApiResources define the apis in your system
    public IEnumerable<ApiResource> GetApis()
    {
      return new List<ApiResource>
            {
                new ApiResource("bathhouse", "Bathhouse Service")
            };
    }

    // Identity resources are data like user ID, name, or email address of a user
    // see: http://docs.identityserver.io/en/release/configuration/resources.html
    public IEnumerable<IdentityResource> GetResources()
    {
      return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Your role(s)", new List<string>() { "role"})

            };
    }

    // client want to access resources (aka scopes)
    public IEnumerable<Client> GetClients()
    {
      return new List<Client>
            {
                new Client
                {
                    ClientId = _config["Clients:BathhouseApi:ClientId"] ,
                    ClientName = "Bathhouse Swagger UI",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    AllowedCorsOrigins = {_config["Clients:BathhouseApi:Url"] },
                    RedirectUris = { $"{_config["Clients:BathhouseApi:Url"] }/index.html" },
                    PostLogoutRedirectUris = { $"{_config["Clients:BathhouseApi:Url"] }/index.html" },
                    AllowedScopes =
                    {
                        "bathhouse"
                    }
                }
            };
    }
  }
}