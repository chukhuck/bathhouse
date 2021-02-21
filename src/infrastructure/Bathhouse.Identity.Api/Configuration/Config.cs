using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Bathhouse.Identity.Api.Configuration
{
  public class Config
  {
    private readonly IConfiguration _config;

    public Config(IConfiguration config)
    {
      _config = config;
    }


    public IEnumerable<ApiScope> GetApiScopes()
    {
      return new[]
      {
                new ApiScope(name: "bathhouse",   displayName: "Access API Bathhouse")
            };
    }


    // ApiResources define the apis in your system
    public IEnumerable<ApiResource> GetApis()
    {
      return new List<ApiResource>
            {
                new ApiResource("bathhouse", "Bathhouse Service") 
                {
                  Scopes = new []{ "bathhouse" } 
                }
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
                    ClientName = _config["Clients:BathhouseApi:Name"],
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    AllowedCorsOrigins = {_config["Clients:BathhouseApi:Url"] },
                    AllowedScopes =
                    {
                        "bathhouse",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    RedirectUris = { _config["Clients:BathhouseApi:Url"] + "/swagger/oauth2-redirect.html" },
                    AllowOfflineAccess = false,
                    AccessTokenLifetime = 3600,
                    IdentityTokenLifetime = 300,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
            };
    }
  }
}