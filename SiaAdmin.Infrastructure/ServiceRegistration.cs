using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.Excel;
using SiaAdmin.Application.Interfaces.ConvertExcel;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Interfaces.Sms;
using SiaAdmin.Infrastructure.Services;

namespace SiaAdmin.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IExcelService, ExcelService>();
            services.AddSingleton<IConvertExcelFile, ConvertExcelFileService>();
            services.AddSingleton<ISmsService, SmsService>();
        }
    }
}
