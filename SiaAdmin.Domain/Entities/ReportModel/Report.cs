using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Domain.Entities.ReportModel
{
    public class Report
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CategoryId { get; set; }
        public ReportType ReportType { get; set; }
        public string QueryId { get; set; }
        public string Description { get; set; }
        public string DateFieldName { get; set; }
        public string SheetName { get; set; }
        public string FileNamePrefix { get; set; }
        public IDictionary<string, string> ColumnMappings { get; set; } = new Dictionary<string, string>();
        public string CustomSql { get; set; }
    }
}
