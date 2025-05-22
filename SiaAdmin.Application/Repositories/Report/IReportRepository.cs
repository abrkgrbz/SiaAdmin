using SiaAdmin.Domain.Entities.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Repositories.Report
{
    public interface IReportRepository
    {
        Task<IEnumerable<ReportCategory>> GetReportCategoriesAsync();
        Task<Domain.Entities.ReportModel.Report> GetReportByIdAsync(string id);
    }
}
