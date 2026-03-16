using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TiendaUCN.src.Infrastructure.Data;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

#region Database Configuration
Log.Information("Configurando base de datos SQLite");
string connectionStringDB = Environment.GetEnvironmentVariable("DATA_BASE_URL") ?? throw new ArgumentNullException("DataBase name cannot be null or empty");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionStringDB));
#endregion


#region Logging Configuration
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));
#endregion

var app = builder.Build();

app.MapOpenApi();
app.Run();