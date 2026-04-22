using Catalog.API;
using Inventory.API;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Commands.DeleteProduct;
using Ecommerce.API.Caching;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddInventoryModule(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure cache: use Redis if connection string present, otherwise noop implementation
var redisConn = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrWhiteSpace(redisConn))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConn;
        options.InstanceName = "ecommerce:";
    });

    builder.Services.AddScoped<ICacheService, RedisCacheService>();
}
else
{
    // No Redis configured -> register a no-op cache to avoid runtime failures
    builder.Services.AddScoped<ICacheService, Ecommerce.API.Caching.NoopCacheService>();
}
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCommandValidator>();
// Inventory validators
builder.Services.AddValidatorsFromAssemblyContaining<Inventory.Application.Commands.CreateInventoryItem.CreateInventoryItemCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Inventory.Application.Commands.UpdateInventoryItem.UpdateInventoryItemCommandValidator>();
// Add stock/reservation validators
builder.Services.AddValidatorsFromAssemblyContaining<Inventory.Application.Commands.AddStock.AddStockCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Inventory.Application.Commands.ReserveStock.ReserveStockCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Inventory.Application.Commands.ConfirmReservation.ConfirmReservationCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Inventory.Application.Commands.ReleaseReservation.ReleaseReservationCommandValidator>();

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
// Redis configured above conditionally
builder.Host.UseSerilog();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7054";   
        options.Audience = "catalog.api";

        options.RequireHttpsMetadata = false;           

        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        options.MapInboundClaims = false;
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CanUpdateCatalog", p => p.RequireClaim("scope", "catalog.update"))
    .AddPolicy("CatalogRead", policy =>
    {
        policy.RequireClaim("scope", "catalog.read");
    });
;
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();   
app.UseAuthorization();
app.MapControllers();
app.Run();

