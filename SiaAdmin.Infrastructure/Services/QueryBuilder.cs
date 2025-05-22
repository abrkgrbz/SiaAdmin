using SiaAdmin.Application.Interfaces.QueryBuilder;
using SiaAdmin.Domain.Entities.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Infrastructure.Services
{
    public class QueryBuilder : IQueryBuilder
    {

        private readonly IEnumerable<IQueryTypeBuilder> _queryTypeBuilders;

        public QueryBuilder(IEnumerable<IQueryTypeBuilder> queryTypeBuilders)
        {
            _queryTypeBuilders = queryTypeBuilders;
        }

        public string BuildQuery(Report report, string dateRange, string additionalParams)
        { 
            var builder = _queryTypeBuilders
                .FirstOrDefault(b => b.CanHandle(report.ReportType));

            if (builder == null)
            {
                throw new NotSupportedException($"Desteklenmeyen rapor tipi: {report.ReportType}");
            }

            return builder.BuildQuery(report, dateRange, additionalParams);
        }
 
    }
}
