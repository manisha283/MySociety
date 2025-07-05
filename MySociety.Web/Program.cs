using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySociety.Entity.Data;
using MySociety.Service.Configuration;
using MySociety.Web.Extensions;
using MySociety.Web.Hubs;
using MySociety.Web.Middlewares;
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

builder.Services.AddDbContext<MySocietyDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DbConnection")
));

// Add services to the container.
builder.Services.AddControllersWithViews();

//HttpContext
builder.Services.AddHttpContextAccessor();

//Configuration
EmailConfig.LoadEmailConfiguration(builder.Configuration);
JwtConfig.LoadJwtConfiguration(builder.Configuration);

builder.Services.AddProjectServices();

builder.Services.AddSignalR();

//Session 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); // Set session timeout
    options.Cookie.HttpOnly = true; // Ensure session is only accessible via HTTP
    options.Cookie.IsEssential = true;
});

//Authentication
if (string.IsNullOrEmpty(JwtConfig.Key))   // Ensure Key is Not Null or Empty
{
    throw new InvalidOperationException("JWT Secret Key is missing");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Extract token from the "JwtToken" cookie
                var token = context.Request.Cookies["mySocietyAuthToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtConfig.Issuer,
            ValidAudience = JwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Key))
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
    context.Response.Headers.Add("Pragma", "no-cache");
    context.Response.Headers.Add("Expires", "0");

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseStatusCodePagesWithReExecute("/Auth/Error/{0}");
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401 && 
        !context.Request.Path.StartsWithSegments("/api"))
    {
        var returnUrl = context.Request.Path + context.Request.QueryString;
        context.Response.Redirect($"/Auth/Login?returnUrl={Uri.EscapeDataString(returnUrl)}");
    }
});

app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
