using SiaAdmin.Domain.Entities.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.Report
{
    public class ReportDefinitionDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
        public string ReportType { get; set; }
        public string QueryId { get; set; }
        public string SheetName { get; set; }
        public string FileNamePrefix { get; set; }
        public string Description { get; set; }
        public string DateFieldName { get; set; }
        public string CustomSql { get; set; }
        public IDictionary<string, string> ColumnMappings { get; set; } = new Dictionary<string, string>();

        // UI'da gösterilecek bilgiler 
        public bool SupportsCustomDateRange { get; set; }
        public bool UsesTemplate { get; set; }
        public string TemplateName { get; set; }
    }
}
