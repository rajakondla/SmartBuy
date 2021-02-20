using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        // claims related to scopes
        // this returns scopes related to access the Identity
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(), // gives user identifier claim
                new IdentityResources.Profile() // gives profile related claims (like name, family_name)
            };
        }

        // this returns scopes related to access the APIs
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                //new ApiResource("InvestmentManagerAPI","Investment Manager API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "123645",
                    ClientName = "SmartBuyUI",
                    AllowedGrantTypes  = GrantTypes.Code,
                    RequirePkce = true,
                    RequireConsent = true,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44389/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>{
                         "https://localhost:44389/signout-callback-oidc"
                    }
                }
            };
        }
    }
}
