using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace Identity
{
    public class Config
    {

        public static IEnumerable<ApiScope> Scopes =>
    new List<ApiScope>
    {
        new ApiScope("catalog.read"),
        new ApiScope("catalog.create"),
        new ApiScope("catalog.update"),
        new ApiScope("catalog.delete")
    };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
        new Client
        {
            ClientId = "admin-portal",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            AllowOfflineAccess= true,
            AccessTokenLifetime = 900, // 15 mins
            AbsoluteRefreshTokenLifetime = 2592000, // 30 days
            SlidingRefreshTokenLifetime = 1296000,  // 15 days
            RefreshTokenUsage = TokenUsage.ReUse,
            RefreshTokenExpiration = TokenExpiration.Sliding,
            RedirectUris = { "https://localhost:4200/callback" },
            AllowedCorsOrigins = { "https://localhost:4200" },

            AllowedScopes =
            {
                "openid",
                "profile",
                "offline_access",
                "catalog.read",
                "catalog.create",
                "catalog.update",
                "catalog.delete"
            }
        }
            };

        public static List<TestUser> Users =>
    new List<TestUser>
    {
        new TestUser
        {
            SubjectId = "1",
            Username = "admin",
            Password = "password",
            Claims =
            {
                new Claim("role", "Admin")
            }
        },
        new TestUser
        {
            SubjectId = "2",
            Username = "superadmin",
            Password = "password",
            Claims =
            {
                new Claim("role", "SuperAdmin")
            }
        }
    };
        public static IEnumerable<ApiResource> Resources =>
            new List<ApiResource>
            {
        new ApiResource("catalog.api", "Catalog API")
        {
            Scopes = { "catalog.read", "catalog.create", "catalog.update", "catalog.delete" },
            UserClaims = { "role" }
        }
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
       new List<IdentityResource>
       {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
       };
    }

}
