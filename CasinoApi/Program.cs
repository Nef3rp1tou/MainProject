using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Interfaces.IServices;
using CasinoApi.Middlewares;
using CasinoApi.Repositories;
using CasinoApi.Services;
using log4net.Config;
using log4net;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using CasinoApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

// Configure log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));


builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("CasinoApiContext");
    return new SqlConnection(connectionString);
});


builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IGameTransactionRepository, GameTransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();



builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGameTransactionService, GameTransactionService>();
builder.Services.AddScoped<IUserService, UserService>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
