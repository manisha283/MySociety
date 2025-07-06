using Microsoft.AspNetCore.SignalR;
using MySociety.Repository.Implementations;
using MySociety.Repository.Interfaces;
using MySociety.Service.Implementations;
using MySociety.Service.Interfaces;
using MySociety.Web.Hubs;

namespace MySociety.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        //Email Service 
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IHttpService, HttpService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBlockService, BlockService>();
        services.AddScoped<IFloorService, FloorService>();
        services.AddScoped<IHouseService, HouseService>();
        services.AddScoped<IHouseMappingService, HouseMappingService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IVisitorService, VisitorService>();
        services.AddScoped<IVisitorFeedbackService, VisitorFeedbackService>();
        services.AddScoped<INotificationHubService, NotificationHubService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<INoticeService, NoticeService>();
        services.AddScoped<INoticeAudienceMappingService, NoticeAudienceMappingService>();
        services.AddScoped<IAudienceGroupService, AudienceGroupService>();


        services.AddSingleton<IUserIdProvider, UserIdProvider>();

        return services;
    }
}
