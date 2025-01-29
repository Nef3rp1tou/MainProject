using log4net;
using log4net.Config;
using System.Reflection;
using BankingApi.IServices;
using BankingApi.Middleware;
using BankingApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Load log4net configuration
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); 

builder.Services.AddHttpClient<ICallbackService, CallbackService>(); 
builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>(); // Uses modified log4net-based middleware

app.UseAuthorization();

app.MapControllers(); 

app.Run();
