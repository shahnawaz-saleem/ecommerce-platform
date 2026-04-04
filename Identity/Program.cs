using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();
// ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// IdentityServer 
builder.Services.AddIdentityServer().AddDeveloperSigningCredential()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.Scopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddAspNetIdentity<ApplicationUser>();
var app = builder.Build();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await DbSeeder.SeedAsync(userManager);
}
app.Run();