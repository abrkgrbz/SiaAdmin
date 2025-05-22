using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.DTOs.Report;
using SiaAdmin.Application.Interfaces.Report;

namespace SiaAdmin.Application.Features.Queries.Report.GetReportList
{
    public class GetReportListQuery:IRequest<List<ReportCategoryDto>>
    {
        public string CategoryFilter { get; set; }
        public string SearchText { get; set; }
    }

    public class GetReportListQueryHandler : IRequestHandler<GetReportListQuery, List<ReportCategoryDto>>
    {
        private readonly IReportService _reportService;

        public GetReportListQueryHandler(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<List<ReportCategoryDto>> Handle(GetReportListQuery request, CancellationToken cancellationToken)
        {
            var categories = await _reportService.GetReportCategoriesAsync();
             
            if (!string.IsNullOrEmpty(request.CategoryFilter))
            {
                categories = categories.Where(c => c.Name == request.CategoryFilter).ToList();
            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                foreach (var category in categories)
                {
                    category.Reports = category.Reports
                        .Where(r => r.Title.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase) ||
                                    r.Description.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                 
                categories = categories.Where(c => c.Reports.Any()).ToList();
            }

            return categories.ToList();
        }
    }
}
