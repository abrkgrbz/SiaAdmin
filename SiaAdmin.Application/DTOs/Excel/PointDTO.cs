using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.Excel
{
    public class PointDTO:BaseExcelDTO
    {
        public List<int> SurveyID = new List<int>();
        public int CountData { get; set; }
    }
}
