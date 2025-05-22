using SiaAdmin.Domain.Entities.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.QueryBuilder
{
    public interface IQueryBuilder
    {
        string BuildQuery(Domain.Entities.ReportModel.Report report, string dateRange, string additionalParams);

    }
}
