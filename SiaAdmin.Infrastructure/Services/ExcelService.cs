using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.DTOs.Excel;
using SiaAdmin.Application.Enums;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.Excel;

namespace SiaAdmin.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
         

        public DataTable readExcel(IFormFile file)
        {
           
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var stream = file.OpenReadStream();
            IExcelDataReader reader = null;
            if (file.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }

            if (file.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            var dataTable = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                UseColumnDataType = true
            }).Tables[0]; 
            reader.Dispose(); 
            return dataTable;
        }

        public Task<FileResult> downloadExcel(string type)
        {
            throw new NotImplementedException();
        }

        
    }
}
