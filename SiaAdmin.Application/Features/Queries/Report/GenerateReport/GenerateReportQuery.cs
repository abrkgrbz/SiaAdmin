using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.DTOs.Report;
using SiaAdmin.Application.Interfaces.QueryBuilder;
using SiaAdmin.Application.Interfaces.Report;
using SiaAdmin.Application.Interfaces.User;

namespace SiaAdmin.Application.Features.Queries.Report.GenerateReport
{
    public class GenerateReportQuery : IRequest<ReportResultDto>
    {
        public string ReportId { get; set; }
        public string DateRange { get; set; } = "all";
        public Dictionary<string, string> AdditionalParameters { get; set; } = new Dictionary<string, string>();

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GenerateReportQueryHandler : IRequestHandler<GenerateReportQuery, ReportResultDto>
    {
        private readonly IReportService _reportService;  
        public GenerateReportQueryHandler(IReportService reportService, IUserService userService)
        {
            _reportService = reportService;
        }

        public async Task<ReportResultDto> Handle(GenerateReportQuery request, CancellationToken cancellationToken)
        {
            var reportDto = await _reportService.GetReportByIdAsync(request.ReportId);
    
            if (reportDto == null)
            {
                throw new KeyNotFoundException($"Rapor bulunamadı: {request.ReportId}");
            }

            string dateRange = request.DateRange;
            string additionalParams = null;

            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                dateRange = "custom";

                if (request.AdditionalParameters != null && request.AdditionalParameters.Any())
                {
                    additionalParams = JsonSerializer.Serialize(new
                    {
                        startDate = request.StartDate.Value.ToString("yyyy-MM-dd"),
                        endDate = request.EndDate.Value.ToString("yyyy-MM-dd"),
                        parameters = request.AdditionalParameters
                    });
                }
                else
                {
                    additionalParams = JsonSerializer.Serialize(new
                    {
                        startDate = request.StartDate.Value.ToString("yyyy-MM-dd"),
                        endDate = request.EndDate.Value.ToString("yyyy-MM-dd")
                    });
                }
            }
            else if (request.AdditionalParameters != null && request.AdditionalParameters.Any())
            {
                additionalParams = JsonSerializer.Serialize(new { parameters = request.AdditionalParameters });
            }

            byte[] excelData = await _reportService.GenerateReportExcelAsync(request.ReportId, dateRange, additionalParams);

            string fileName = $"{reportDto.FileNamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return new ReportResultDto
            {
                ReportId = request.ReportId,
                ReportTitle = reportDto.Title,
                FileName = fileName,
                ExcelData = excelData,
                GeneratedAt = DateTime.Now,
                FileSize = excelData.Length
            };
        }
    }
}
