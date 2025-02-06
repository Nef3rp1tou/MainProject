using CasinoApi.Interfaces.IRepositories;
using CasinoApi.Interfaces.IServices;
using CasinoApi.Middlewares;
using CasinoApi.Repositories;
using CasinoApi.Services;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

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

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
