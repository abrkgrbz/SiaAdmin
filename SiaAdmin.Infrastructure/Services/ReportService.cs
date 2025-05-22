using SiaAdmin.Application.Interfaces.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.Report;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Interfaces.QueryBuilder;
using SiaAdmin.Application.Repositories.Report;
using SiaAdmin.Application.Interfaces.Database;

namespace SiaAdmin.Infrastructure.Services
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IExcelService _excelService;
        private readonly IQueryBuilder _queryBuilder; 
        private readonly IDatabaseService _databaseService;

        public ReportService(IReportRepository reportRepository, IExcelService excelService, IQueryBuilder queryBuilder, IDatabaseService databaseService)
        {
            _reportRepository = reportRepository;
            _excelService = excelService;
            _queryBuilder = queryBuilder;
            _databaseService = databaseService;
        }

        public async Task<IEnumerable<ReportCategoryDto>> GetReportCategoriesAsync()
        {
            var categories = await _reportRepository.GetReportCategoriesAsync();
             
            return categories.Select(c => new ReportCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon,
                Reports = c.Reports.Select(r => new ReportDefinitionDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Category = c.Name,
                    Icon = r.ReportType.ToString().ToLower() == "custom" ? "fa-file-alt" : $"fa-{r.ReportType.ToString().ToLower()}",
                    ReportType = r.ReportType.ToString(),
                    QueryId = r.QueryId,
                    SheetName = r.SheetName,
                    FileNamePrefix = r.FileNamePrefix,
                    Description = r.Description,
                    DateFieldName = r.DateFieldName,
                    CustomSql = r.CustomSql,
                    ColumnMappings = r.ColumnMappings
                }).ToList()
            }).ToList();
        }

        public async Task<ReportDefinitionDto> GetReportByIdAsync(string id)
        {
            var report = await _reportRepository.GetReportByIdAsync(id);
            if (report == null)
            {
                return null;
            }

            var categories = await _reportRepository.GetReportCategoriesAsync();
            var category = categories.FirstOrDefault(c => c.Id == report.CategoryId);

            return new ReportDefinitionDto
            {
                Id = report.Id,
                Title = report.Title,
                Category = category?.Name,
                Icon = report.ReportType.ToString().ToLower() == "custom" ? "fa-file-alt" : $"fa-{report.ReportType.ToString().ToLower()}",
                ReportType = report.ReportType.ToString(),
                QueryId = report.QueryId,
                SheetName = report.SheetName,
                FileNamePrefix = report.FileNamePrefix,
                Description = report.Description,
                DateFieldName = report.DateFieldName,
                CustomSql = report.CustomSql,
                ColumnMappings = report.ColumnMappings
            };
        }

        public async Task<DataTable> GetReportDataAsync(ReportDefinitionDto reportDto, string dateRange, string additionalParams)
        { 
            var report = await _reportRepository.GetReportByIdAsync(reportDto.Id);
            if (report == null)
            {
                throw new KeyNotFoundException($"Rapor bulunamadı: {reportDto.Id}");
            }
             
            string query = _queryBuilder.BuildQuery(report, dateRange, additionalParams);
             
            return await _databaseService.ExecuteQueryAsync(query);
        }

        public async Task<byte[]> GenerateReportExcelAsync(string reportId, string dateRange, string additionalParams)
        { 
            var reportDto = await GetReportByIdAsync(reportId);
            if (reportDto == null)
            {
                throw new KeyNotFoundException($"Rapor bulunamadı: {reportId}");
            }
             
            var data = await GetReportDataAsync(reportDto, dateRange, additionalParams);
            if (data == null || data.Rows.Count == 0)
            {
                throw new InvalidOperationException("Seçilen parametrelerle veri bulunamadı.");
            }
             
            var report = await _reportRepository.GetReportByIdAsync(reportId);
             
            byte[] excelData; 
            if (reportDto.ReportType.ToLower() == "custom" && reportDto.Id == "601")  
            {
                excelData = await _excelService.GenerateExcelFromTemplateAsync(data, report, "TemplateTanisma.xlsx");
            }
            else
            {
                excelData = await _excelService.GenerateExcelAsync(data, report);
            }

            return excelData;
        }
    }
}
