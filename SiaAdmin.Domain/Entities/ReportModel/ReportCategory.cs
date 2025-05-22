using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Domain.Entities.ReportModel
{
    public class ReportCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
