using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resend;
using Serilog;
using System.Text;
using Tienda_UCN_api.Src.Application.Mappers;
using TiendaUCN.src.API.Middlewares;
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

var app = builder.Build();

#region Database Migration and Mapster Configuration
Log.Information("Aplicando migraciones a la base de datos");
using (var scope = app.Services.CreateScope())
{
    await DataSeeder.Initialize(scope.ServiceProvider);

    // Configurar los mapeos de Mapster
    MapperExtensions.ConfigureMapster(scope.ServiceProvider);
}
#endregion

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();         // 1 — valida el JWT
app.UseMiddleware<BlacklistMiddleware>(); // 2 — verifica blacklist
app.UseAuthorization();          // 3 — verifica roles y permisos
app.MapOpenApi();
app.MapControllers();
app.Run();