using System;
using System.Text;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartTourism360.Infrastructure.Data;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Infrastructure.Services;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Api.Middlewares;
using SmartTourism360.Api.Transformers;
using Scalar.AspNetCore;
using Minio;
using SmartTourism360.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with PostgreSQL and NetTopologySuite
builder.Services.AddDbContext<SmartTourism360DbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseNetTopologySuite()
              .MigrationsAssembly("SmartTourism360.Api")
              .EnableRetryOnFailure(
                  maxRetryCount: 10,
                  maxRetryDelay: TimeSpan.FromSeconds(5),
                  errorCodesToAdd: null)));

// Register Scoped Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();

// Register MinIo Configuration Options
builder.Services.Configure<MinIoOptions>(builder.Configuration.GetSection(MinIoOptions.MinIo));

// Register Minio Client as a Singleton
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var minioConfig = builder.Configuration.GetSection("MinIo");
    var secure = minioConfig.GetValue<bool>("Secure");
    return new MinioClient()
        .WithEndpoint(minioConfig["Endpoint"] ?? "localhost:9000")
        .WithCredentials(minioConfig["AccessKey"] ?? "minioadmin", minioConfig["SecretKey"] ?? "minioadmin_secure_pass")
        .WithSSL(secure)
        .Build();
});

builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IVirtualTourService, VirtualTourService>();
builder.Services.AddScoped<IPanoramaService, PanoramaService>();
builder.Services.AddScoped<IHotspotService, HotspotService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Add services to the container and customize Validation Error Response
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .Select(e => new ValidationError
                {
                    Field = JsonNamingPolicy.CamelCase.ConvertName(e.Key),
                    Message = e.Value!.Errors.First().ErrorMessage
                })
                .ToList();

            var response = ApiResponse<object>.FailureResponse("Dữ liệu đầu vào không hợp lệ.", errors);
            return new BadRequestObjectResult(response);
        };
    });

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var keyString = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
var key = Encoding.UTF8.GetBytes(keyString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Configure built-in OpenAPI generator with security transformer
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

// Global Exception Handler
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Maps /openapi/v1.json
    app.MapScalarApiReference(); // Maps Scalar UI at /scalar/v1
    
    // Auto-migrate and seed data on startup in Development
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<SmartTourism360DbContext>();
            var storageService = services.GetRequiredService<IStorageService>();
            await SmartTourism360DbContextSeed.SeedAsync(context, storageService);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during database migration or seeding.");
        }
    }
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
