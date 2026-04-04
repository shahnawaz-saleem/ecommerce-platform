using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System.Security.Claims;

namespace Identity
{
    public class CustomProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var role = context.Subject.FindFirst("role")?.Value;

            var allowedScopes = role switch
            {
                "Customer" => new[] { "catalog.read" },

                "Admin" => new[]
                {
                "catalog.read",
                "catalog.create",
                "catalog.update"
            },

                "SuperAdmin" => new[]
                {
                "catalog.read",
                "catalog.create",
                "catalog.update",
                "catalog.delete"
            },

                _ => Array.Empty<string>()
            };

            // 🔥 Requested scopes from client
            var requestedScopes = context.RequestedResources.RawScopeValues;

            // 🔥 Final scopes = intersection
            var finalScopes = requestedScopes.Intersect(allowedScopes);

            // Add scopes to token
            context.IssuedClaims.Add(
                new Claim("scope", string.Join(" ", finalScopes))
            );

            // Add role
            context.IssuedClaims.Add(new Claim("role", value: role??"null"));

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
