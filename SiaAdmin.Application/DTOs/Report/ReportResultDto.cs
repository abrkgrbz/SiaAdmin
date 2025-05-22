using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.Report
{
    public class ReportResultDto
    {
        public string ReportId { get; set; }
        public string ReportTitle { get; set; }
        public string FileName { get; set; }
        public byte[] ExcelData { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public string GeneratedBy { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    }
}
