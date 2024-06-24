using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExcelService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

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

        public async Task<byte[]> downloadExcel(string type)
        {
            string path="";
            byte[] bytes;
            switch (type)
            {
                case nameof(ExcelTable.InternalGUID):
                    path = Path.Combine(_webHostEnvironment.WebRootPath, "excel-files") + "\\InternalGUID.xlsx";
                    bytes = await System.IO.File.ReadAllBytesAsync(path);
                    return bytes;
                    break;
                case nameof(ExcelTable.SurveyUserGUID):
                    path = Path.Combine(_webHostEnvironment.WebRootPath, "excel-files") + "\\SurveyUserGUID.xlsx";
                    bytes = await System.IO.File.ReadAllBytesAsync(path);
                    return bytes;
                    break;
                case nameof(ExcelTable.Point):
                    path = Path.Combine(_webHostEnvironment.WebRootPath, "excel-files") + "\\Point.xlsx";
                    bytes = await System.IO.File.ReadAllBytesAsync(path);
                    return bytes;
                    break;
            }
            return null;
        }


    }
}
