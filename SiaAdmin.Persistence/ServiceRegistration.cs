using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SiaAdmin.Application.Interfaces;
using SiaAdmin.Application.Interfaces.NotificationProcessor;
using SiaAdmin.Application.Interfaces.User;
using SiaAdmin.Application.ProcedureRepositories.PanelistSaatKullanimi;
using SiaAdmin.Application.ProcedureRepositories.TanitimAnketiDolduran;
using SiaAdmin.Application.ProcedureRepositories.ToplamAnketBilgisi;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Application.Repositories.NotificationFailures;
using SiaAdmin.Application.Repositories.NotificationHistory;
using SiaAdmin.Application.Repositories.NotificationScheduledDeviceTokens;
using SiaAdmin.Application.Repositories.OTPHistory;
using SiaAdmin.Application.Repositories.Report;
using SiaAdmin.Persistence.Contexts;
using SiaAdmin.Persistence.ProcedureRepositories.PanelistSaatKullanimi;
using SiaAdmin.Persistence.ProcedureRepositories.TanitimAnketiDolduran;
using SiaAdmin.Persistence.ProcedureRepositories.ToplamAnketBilgisi;
using SiaAdmin.Persistence.Repositories;
using SiaAdmin.Persistence.Repositories.DeviceRegistrations;
using SiaAdmin.Persistence.Repositories.NotificationFailures;
using SiaAdmin.Persistence.Repositories.NotificationHistory;
using SiaAdmin.Persistence.Repositories.NotificationScheduledDeviceTokens;
using SiaAdmin.Persistence.Repositories.OTPHistory;
using SiaAdmin.Persistence.Repositories.Report;
using SiaAdmin.Persistence.Repositories.UserLog;
using SiaAdmin.Persistence.Services;

namespace SiaAdmin.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<SiaAdminDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));
            services.AddScoped<IBlockListReadRepository, BlockListReadRepository>();
            services.AddScoped<IBlockListWriteRepository, BlockListWriteRepository>();
            services.AddScoped<IEODPTableReadRepository, EODPTableReadRepository>();
            services.AddScoped<IEODTableReadRepository, EODTableReadRepository>();
            services.AddScoped<IFilterDataReadRepository, FilterDataReadRepository>();
            services.AddScoped<IIncentiveReadRepository, IncentiveReadRepository>();
            services.AddScoped<IIncentiveWriteRepository, IncentiveWriteRepository>();
            services.AddScoped<ISiaRoleReadRepository, SiaRoleReadRepository>();
            services.AddScoped<ISiaUserReadRepository, SiaUserReadRepository>();
            services.AddScoped<ISiaUserWriteRepository, SiaUserWriteRepository>();
            services.AddScoped<ISiaUserRoleReadRepository, SiaUserRoleReadRepository>();
            services.AddScoped<ISiaUserRoleWriteRepository, SiaUserRoleWriteRepository>();
            services.AddScoped<ISurveyReadRepository, SurveyReadRepository>();
            services.AddScoped<ISurveyWriteRepository, SurveyWriteRepository>();
            services.AddScoped<ISurveyAssignedReadRepository, SurveyAssignedReadRepository>();
            services.AddScoped<ISurveyAssignedWriteRepository, SurveyAssignedWriteRepository>();
            services.AddScoped<IOTPHistoryReadRepository, OTPHistoryReadRepository>();
            services.AddScoped<IOTPHistoryWriteRepository, OTPHistoryWriteRepository>();
            services.AddScoped<ISurveyLogReadRepository, SurveyLogReadRepository>();
            services.AddScoped<ISurveyLogWriteRepository, SurveyLogWriteRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IWaitDataReadRepository, WaitDataReadRepository>();
            services.AddScoped<IUserLogReadRepository, UserLogReadRepository>();
            services.AddScoped<IUserLogWriteRepository, UserLogWriteRepository>();
            services.AddScoped<IDeviceRegistrationsWriteRepository, DeviceRegistrationsWriteRepository>();
            services.AddScoped<IDeviceRegistrationsReadRepository, DeviceRegistrationsReadRepository>();
            services.AddScoped<INotificationHistoryWriteRepository, NotificationHistoryWriteRepository>();
            services.AddScoped<INotificationHistoryReadRepository, NotificationHistoryReadRepository>();
            services.AddScoped<INotificationFailuresReadRepository, NotificationFailuresReadRepository>();
            services.AddScoped<INotificationFailuresWriteRepository, NotificationFailuresWriteRepository>();
            services.AddScoped<INotificationScheduledDeviceTokensReadRepository, NotificationScheduledDeviceTokensReadRepository>();
            services.AddScoped<INotificationScheduledDeviceTokensWriteRepository, NotificationScheduledDeviceTokensWriteRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddScoped(typeof(IStoredProcedureRepository<>), typeof(StoredProcedureRepository<>));

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IGetTotalSurveyInformation, GetTotalSurveyInformation>();
            services.AddScoped<IGetPerformancePanelistPerformance, GetPerformancePanelistPerformance>();
            services.AddScoped<IGetNumberOfFillingSurveyIntro, GetNumberOfFillingSurveyIntro>();
        }
    }
}
