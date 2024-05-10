using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Persistence.Contexts;
using SiaAdmin.Persistence.Repositories;

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
            services.AddScoped<ISurveyLogReadRepository, SurveyLogReadRepository>();
            services.AddScoped<ISurveyLogWriteRepository, SurveyLogWriteRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IWaitDataReadRepository, WaitDataReadRepository>();

        }
    }
}
