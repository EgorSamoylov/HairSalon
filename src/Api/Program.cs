using Api.Configuration;
using Api.ExceptionHandlers;
using Api.Middleware;
using Application;
using Infrastructure;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var logPattern = @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [ClientIp={ClientIp}] {Message:lj}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.WithClientIp()
    .WriteTo.Console(outputTemplate: logPattern)
    .WriteTo.File(Path.Combine("logs", "quiz-backend-.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        rollOnFileSizeLimit: true,
        outputTemplate: logPattern)
.CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "HairSalon API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Authorization: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             Array.Empty<string>()
         }
     });
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();
builder.Services.AddExceptionHandler<AuthServiceExceptionHandler>();
builder.Services.AddExceptionHandler<DbExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettings);
var secret = jwtSettings["Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");

// Add JWT Authentication
builder.Services.AddAuthentication("HttponlyAuth")
    .AddCookie("HttponlyAuth", options =>
    {
        options.Cookie.HttpOnly = false;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/Auth/login";

        // Îòêëþ÷àåì ðåäèðåêò è âîçâðàùàåì 401
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("X-Auth-Redirect", "/login");
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;

    options.AddFixedWindowLimiter("auth", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 5;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});

builder.Services.AddCors(
    (options) =>
    {
        options.AddPolicy("AllowLocalhost", policy =>
        {
            policy.WithOrigins("localhost", "http://localhost:3000", "http://158.160.177.163")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();

        });
    }
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
    migrationRunner.Run();
}

app.UseMiddleware<PerformanceMiddleware>(TimeSpan.FromMilliseconds(700));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");

app.UseRateLimiter();
app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
