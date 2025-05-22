using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.ReportModel;

namespace SiaAdmin.Application.Interfaces.QueryBuilder
{
    public interface IQueryTypeBuilder
    { 
        bool CanHandle(ReportType reportType); 
        string BuildQuery(Domain.Entities.ReportModel.Report report, string dateRange, string additionalParams);
    }
}
