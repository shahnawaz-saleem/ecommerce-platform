using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.Services;

var builder = WebApplication.CreateBuilder(args);



// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();



// CORS for SPA clients
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:7128")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
    options.AddPolicy("AllowPostman", policy =>
        policy.WithOrigins("https://oauth.pstmn.io")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


// ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// custom profile service
builder.Services.AddScoped<IProfileService, CustomProfileService>();


// IdentityServer 
builder.Services.AddIdentityServer(options =>
{
    options.UserInteraction.LoginUrl = "/Account/LoginModel";
}).AddDeveloperSigningCredential()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.Scopes)
    .AddInMemoryIdentityResources(Config.IdentityResources).AddInMemoryApiResources(Config.Resources)
    .AddAspNetIdentity<ApplicationUser>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapGet("/", (HttpContext ctx) =>
{
    return Results.Redirect("/.well-known/openid-configuration");
});
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await DbSeeder.SeedAsync(userManager, roleManager);
}
app.MapDefaultControllerRoute();
app.Run();