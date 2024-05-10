using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.DTOs.Excel;
using SiaAdmin.Application.Enums;

namespace SiaAdmin.Application.Interfaces.Excel
{
    public interface IExcelService
    { 
        DataTable readExcel(IFormFile file);
        Task<FileResult> downloadExcel(string type);

    

    }
}
