using DotNetEnv;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resend;
using Serilog;
using System.Text;
using Tienda_UCN_api.Src.Application.Mappers;
using TiendaUCN.src.API.Middlewares;
using TiendaUCN.src.Application.Jobs.Implements;
using TiendaUCN.src.Application.Jobs.Interfaces;
using TiendaUCN.src.Application.Mappers;
using TiendaUCN.src.Application.Services.Implements;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Implements;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Configuración de mapeadores
builder.Services.AddScoped<UserMapper>();

// Configuración de servicios y repositorios
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
builder.Services.AddScoped<IUserJob, UserJob>();

#region Email Service Configuration
Log.Information("Configurando servicio de correo electrónico Resend");
builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = Environment.GetEnvironmentVariable("RESEND_API_KEY") ?? throw new ArgumentNullException("RESEND_API_KEY is not set");
});
builder.Services.AddTransient<IResend, ResendClient>();
#endregion

#region Authentication Configuration
Log.Information("Configurando autenticación JWT");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    ).AddJwtBearer(options =>
    {
        string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("La clave secreta JWT no está configurada.");
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true, // Valida la expiración del token
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero //Sin tolerencia a tokens expirados
        };
    });
#endregion

#region Database Configuration
Log.Information("Configurando base de datos SQLite");
string connectionStringDB = Environment.GetEnvironmentVariable("DATA_BASE_URL") ?? throw new ArgumentNullException("DATA_BASE_URL is not set");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionStringDB));
#endregion

#region Logging Configuration
Log.Information("Configurando Serilog para logging");
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));
#endregion

#region Hangfire Configuration
Log.Information("Configurando los trabajos en segundo plano de Hangfire");
var cronExpressionDeleteUnconfirmedUsers = builder.Configuration["Jobs:CronJobDeleteUnconfirmedUsers"] ?? throw new InvalidOperationException("La expresión cron para eliminar usuarios no confirmados no está configurada.");
var cronExpressionDeleteExpiredTokens = builder.Configuration["Jobs:CronJobDeleteExpiredTokens"] ?? throw new InvalidOperationException("La expresión cron para eliminar tokens expirados no está configurada.");
var timeZone = TimeZoneInfo.FindSystemTimeZoneById(builder.Configuration["Jobs:TimeZone"] ?? throw new InvalidOperationException("La zona horaria para los trabajos no está configurada."));
builder.Services.AddHangfire(configuration =>
{
    var connectionStringBuilder = new SqliteConnectionStringBuilder(connectionStringDB);
    var databasePath = connectionStringBuilder.DataSource;

    configuration.UseSQLiteStorage(databasePath);
    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
    configuration.UseSimpleAssemblyNameTypeSerializer();
    configuration.UseRecommendedSerializerSettings();
});
builder.Services.AddHangfireServer();
#endregion

var app = builder.Build();

#region Hangfire Dashboard Configuration
Log.Information("Configurando el panel de control de Hangfire");
app.UseHangfireDashboard(builder.Configuration["HangfireDashboard:DashboardPath"] ?? throw new InvalidOperationException("La ruta de hangfire no ha sido declarada"), new DashboardOptions
{
    StatsPollingInterval = builder.Configuration.GetValue<int?>("HangfireDashboard:StatsPollingInterval") ?? throw new InvalidOperationException("El intervalo de actualización de estadísticas del panel de control de Hangfire no está configurado."),
    DashboardTitle = builder.Configuration["HangfireDashboard:DashboardTitle"] ?? throw new InvalidOperationException("El título del panel de control de Hangfire no está configurado."),
    DisplayStorageConnectionString = builder.Configuration.GetValue<bool?>("HangfireDashboard:DisplayStorageConnectionString") ?? throw new InvalidOperationException("La configuración 'HangfireDashboard:DisplayStorageConnectionString' no está definida."),
});
#endregion

#region Database Seeding and Mapster Configuration
Log.Information("Aplicando migraciones a la base de datos y configurando mapeadores de Mapster");
using (var scope = app.Services.CreateScope())
{
    // Poblar la base de datos
    await DataSeeder.Initialize(scope.ServiceProvider);

    // Configurar los mapeos de Mapster
    MapperExtensions.ConfigureMapster(scope.ServiceProvider);
}
#endregion

# region Configuración de trabajos recurrentes de Hangfire
Log.Information("Configurando trabajos recurrentes de Hangfire");
// Configurar el trabajo para eliminar los usuarios no confirmados
var jobId = nameof(UserJob.DeleteUnconfirmedUsersAsync);
RecurringJob.AddOrUpdate<UserJob>(
    jobId,
    job => job.DeleteUnconfirmedUsersAsync(),
    cronExpressionDeleteUnconfirmedUsers,
    new RecurringJobOptions
    {
        TimeZone = timeZone
    }
);
Log.Information($"Job recurrente '{jobId}' configurado con cron: {cronExpressionDeleteUnconfirmedUsers} en zona horaria: {timeZone.Id}");

// Configurar el trabajo para eliminar los tokens expirados en la blacklist
jobId = nameof(UserJob.DeleteExpiredTokensInBlacklistAsync);
RecurringJob.AddOrUpdate<UserJob>(
    jobId,
    job => job.DeleteExpiredTokensInBlacklistAsync(),
    cronExpressionDeleteExpiredTokens,
    new RecurringJobOptions
    {
        TimeZone = timeZone
    }
);
Log.Information($"Job recurrente '{jobId}' configurado con cron: {cronExpressionDeleteExpiredTokens} en zona horaria: {timeZone.Id}");
#endregion

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();         // 1 — valida el JWT
app.UseMiddleware<BlacklistMiddleware>(); // 2 — verifica blacklist
app.UseAuthorization();          // 3 — verifica roles y permisos
app.MapOpenApi();
app.MapControllers();
app.Run();