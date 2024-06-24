using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.ExcelFile.GetExcelFile
{
    public class GetExcelFileResponse
    {
        public byte[] excelFile { get; set; }
        public string excelFileName { get; set; }
    }
}
