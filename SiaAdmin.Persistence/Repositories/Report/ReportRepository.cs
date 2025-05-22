using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories.Report;
using SiaAdmin.Domain.Entities.ReportModel;

namespace SiaAdmin.Persistence.Repositories.Report
{
    public class ReportRepository:IReportRepository
    {
        public async Task<IEnumerable<ReportCategory>> GetReportCategoriesAsync()
        { 
            var categories = new List<ReportCategory>
            {
                new ReportCategory
                {
                    Id = "1",
                    Name = "Müşteri",
                    Icon = "fa-smile-beam",
                    Reports = new List<Domain.Entities.ReportModel.Report>
                    {
                        new Domain.Entities.ReportModel.Report
                        {
                            Id = "101",
                            Title = "Müşteri Memnuniyet Anketi",
                            CategoryId = "1",
                            ReportType = ReportType.SSI,
                            QueryId = "101",
                            SheetName = "Müşteri Anketi",
                            FileNamePrefix = "Musteri_Memnuniyet",
                            Description = "Müşteri memnuniyet anketinin ham verilerini içeren rapor.",
                            DateFieldName = "sys_StartTimeStamp"
                        }
                    }
                },
                new ReportCategory
                {
                    Id = "6",
                    Name = "TanismaSSIData",
                    Icon = "fa-poll",
                    Reports = new List<Domain.Entities.ReportModel.Report>
                    {
                        new Domain.Entities.ReportModel.Report
                        {
                            Id = "601",
                            Title = "TanismaSSIData Rapor Listesi",
                            CategoryId = "6",
                            ReportType = ReportType.Custom,
                            QueryId = "tanisma",
                            SheetName = "TANISMA",
                            FileNamePrefix = "TanismaAnketi",
                            Description = "Tanışma anketi sonuçlarını içeren detaylı rapor.",
                            DateFieldName = "sys_StartTimeStamp" 
                        }
                    }
                } 
            };

            return categories;
        }

        public async Task<Domain.Entities.ReportModel.Report> GetReportByIdAsync(string id)
        {
            var allCategories = await GetReportCategoriesAsync();
            return allCategories.SelectMany(c => c.Reports).FirstOrDefault(r => r.Id == id);
        }
    }
}
