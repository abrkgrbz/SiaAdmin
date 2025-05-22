using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.Report;

namespace SiaAdmin.Application.Interfaces.Report
{
    public interface IReportService
    {
        Task<IEnumerable<ReportCategoryDto>> GetReportCategoriesAsync();
        Task<ReportDefinitionDto> GetReportByIdAsync(string id);
        Task<DataTable> GetReportDataAsync(ReportDefinitionDto report, string dateRange, string additionalParams);
        Task<byte[]> GenerateReportExcelAsync(string reportId, string dateRange, string additionalParams);

    }
}
