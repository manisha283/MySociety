using Microsoft.EntityFrameworkCore;
using MySociety.Entity.Data;
using MySociety.Repository.Implementations;
using MySociety.Repository.Interfaces;
using MySociety.Service.Configuration;
using MySociety.Service.Implementations;
using MySociety.Service.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "MySociety-log.txt");
 
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .Enrich.FromLogContext()
    .WriteTo.File(
        path: logFilePath,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        fileSizeLimitBytes: 10_000_000,
        rollOnFileSizeLimit: true,
        shared: true,
        outputTemplate: "[{NewLine}{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}"
    )
    .CreateLogger();

builder.Logging.AddSerilog(Log.Logger);

#region Service

builder.Services.AddDbContext<MySocietyDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DbConnection")
));

// Add services to the container.
builder.Services.AddControllersWithViews();

//HttpContext
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//Email Service 
EmailConfig.LoadEmailConfiguration(builder.Configuration);
builder.Services.AddScoped<IEmailService, EmailService>();

//Jwt Service
JwtConfig.LoadJwtConfiguration(builder.Configuration);
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddScoped<IFloorService, FloorService>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<IRoleService, RoleService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
