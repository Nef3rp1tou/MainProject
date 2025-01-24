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


var builder = WebApplication.CreateBuilder(args);

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("MvcProjectContextConnection")
    ?? throw new InvalidOperationException("Connection string 'MvcProjectContextConnection' not found.");

// Add DbContext for Identity
builder.Services.AddDbContext<MvcProjectContext>(options =>
     options.UseSqlServer(connectionString));

// Register repositories and services
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();

builder.Services.AddScoped<IDepositWithdrawRequestsRepository, DepositWithdrawRequestsRepository>();
builder.Services.AddScoped<IDepositWithdrawRequestsService, DepositWithdrawRequestsService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IBankingApiService, BankingApiService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Bind and validate BankingApiConfig
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

// Add HTTP client for API communication
builder.Services.AddHttpClient();

// Register Identity services with roles
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MvcProjectContext>();

// Register Razor Pages and Controllers
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Register Dapper's DB connection for repositories
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(connectionString));

var app = builder.Build();

// Configure middleware
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map routes
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeeder.SeedRolesAndAdminAsync(services);
}

app.Run();
