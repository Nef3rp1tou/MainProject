using log4net;
using log4net.Config;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using MvcProject.Areas.Identity.Data;
using MvcProject.Interfaces.IRepositories;
using MvcProject.Interfaces.IServices;
using MvcProject.Repositories;
using MvcProject.Services;
using System.Data;
using MvcProject.Settings;
using Microsoft.Extensions.Options;
using MvcProject.Middleware;
using MvcProject.Middlewares;

var builder = WebApplication.CreateBuilder(args);

string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

// Configure log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));


var connectionString = builder.Configuration.GetConnectionString("MvcProjectContextConnection")
    ?? throw new InvalidOperationException("Connection string 'MvcProjectContextConnection' not found.");

builder.Services.AddDbContext<MvcProjectContext>(options =>
     options.UseSqlServer(connectionString));


builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();


builder.Services.AddScoped<IDepositWithdrawRequestsRepository, DepositWithdrawRequestsRepository>();
builder.Services.AddScoped<IDepositWithdrawRequestsService, DepositWithdrawRequestsService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();


builder.Services.AddScoped<ITransactionsHandlerService, TransactionsHandlerService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICallbackService, CallbackService>();
builder.Services.AddScoped<IBankingApiService, BankingApiService>();
builder.Services.AddScoped<IUserTokenService, UserTokenService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BankingApiConfig>(builder.Configuration.GetSection("BankingApi"));
builder.Services.AddSingleton(resolver =>
{
    var config = resolver.GetRequiredService<IOptions<BankingApiConfig>>().Value;
    if (string.IsNullOrWhiteSpace(config.BaseUrl) || string.IsNullOrWhiteSpace(config.MerchantId) || string.IsNullOrWhiteSpace(config.SecretKey))
    {
        throw new InvalidOperationException("Banking API configuration is invalid. Ensure BaseUrl, MerchantId, and SecretKey are provided.");
    }
    return config;
});

builder.Services.AddHttpClient();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MvcProjectContext>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeeder.SeedRolesAndAdminAsync(services);
}

app.Run();
