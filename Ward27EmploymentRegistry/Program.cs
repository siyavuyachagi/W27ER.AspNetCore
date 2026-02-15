using Asp.Versioning;
using Domain.Entities;
using Domain.Entities.Identity;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using Ward27EmploymentRegistry.Dependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
// Swagger UI (with OpenApi)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "W27ER Api",
        Version = "v1"
    });

    // Optional: Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});


// API VERSIONING
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
});




// ADD DB_CONTEXT with SQL Server (or any provider)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("MySqlServer")
        ?? throw new InvalidOperationException("Missing database string in configuration.");
    options.UseSqlServer(connectionString, sqlServerOptionsAction =>
    {
        sqlServerOptionsAction.MigrationsAssembly("Ward27EmploymentRegistry");
        sqlServerOptionsAction.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
        sqlServerOptionsAction.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); // Perfomance improvement
    });
    //options.EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
});


// ADD IDENTITY
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "0123456789";
})
.AddRoles<Role>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddSignInManager();





// MAP `appsettings.json` CONFIGURATIONS
builder.Services.Configure<CloudinaryStorageSettings>(builder.Configuration.GetSection(nameof(CloudinaryStorageSettings)));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));
//builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection(nameof(EncryptionSettings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection(nameof(RedisSettings)));


builder.Services.AddHttpContextAccessor();
// All applicatio repositories & services
builder.Services.AddIServices(builder.Configuration);


// ADD AUTHENTICATION
var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
        ?? throw new InvalidOperationException("Missing JwtSettings configuration.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Jwt AUthentication
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudiences = jwtSettings.Audiences,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            AuthenticationType = JwtRegisteredClaimNames.Sub
        };
    });



// ADD CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(jwtSettings.Audiences.ToArray() ?? Array.Empty<string>())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


// ADD RATE LIMITER
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? "anonymous",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    // Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseCors("AllowedOrigins"); // Use Cors
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
