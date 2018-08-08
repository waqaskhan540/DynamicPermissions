using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace DynamicPermissions.Config
{
    public static class IdentityServerConfig
    {
        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };


        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "Demo API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ro.client",
                    ClientName = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "openid", "profile", "email", "api" },
                    AccessTokenLifetime = 86400,                    
                    AccessTokenType = AccessTokenType.Jwt,
                    AlwaysIncludeUserClaimsInIdToken = true,                   
                    IdentityTokenLifetime = 2629746,
                    AlwaysSendClientClaims = true,
                    Enabled = true

                }
            };
        }

       public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    Username = "admin",
                    SubjectId = Guid.NewGuid().ToString(),
                    Password = "password",
                    Claims  = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Role,"admin")
                    }
                },
                new TestUser
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "freeuser",
                    Password = "password",
                    Claims  = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Role,"freeuser")
                    }
                },
                new TestUser
                {
                    Username = "paiduser",
                    Password = "password",
                    Claims  = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Role,"paiduser")
                    }
                },
                new TestUser
                {
                    Username = "visitor",
                    Password = "password",
                    Claims  = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role,"visitor")
                    }
                }
            };
        }
    }
}
