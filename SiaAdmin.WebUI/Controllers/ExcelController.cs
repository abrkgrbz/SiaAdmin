
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SiaAdmin.Application.Features.Queries.ExcelFile.GetExcelFile;
using Bold = DocumentFormat.OpenXml.Spreadsheet.Bold;
using Border = DocumentFormat.OpenXml.Spreadsheet.Border;
using BottomBorder = DocumentFormat.OpenXml.Spreadsheet.BottomBorder;
using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
using Font = DocumentFormat.OpenXml.Spreadsheet.Font;
using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
using FontSize = DocumentFormat.OpenXml.Spreadsheet.FontSize;
using HorizontalAlignmentValues = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues;
using LeftBorder = DocumentFormat.OpenXml.Spreadsheet.LeftBorder;
using NumberingFormat = DocumentFormat.OpenXml.Spreadsheet.NumberingFormat;
using RightBorder = DocumentFormat.OpenXml.Spreadsheet.RightBorder;
using TopBorder = DocumentFormat.OpenXml.Spreadsheet.TopBorder;
using VerticalAlignmentValues = DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues;
using SiaAdmin.Application.Features.Queries.Report.GetReportList;
using SiaAdmin.Application.Features.Queries.Report.GenerateReport;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class ExcelController : BaseController
    {
        private readonly IConfiguration _configuration;

        public ExcelController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<FileResult> FileDownload(GetExcelFileRequest request)
        {
            var response = await Mediator.Send(request);
            return File(response.excelFile, "application/octet-stream", response.excelFileName);
        }


        public async Task<IActionResult> DownloadPage(string categoryFilter = null, string searchText = null)
        {
            var query = new GetReportListQuery
            {
                CategoryFilter = categoryFilter,
                SearchText = searchText
            };

            var categories = await Mediator.Send(query);

            return View(categories);
        }

        public async Task<IActionResult> DownloadReport(GenerateReportQuery query)
        {
            try
            {
                var result = await Mediator.Send(query);

                return File(result.ExcelData, result.ContentType, result.FileName);
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Hata: {ex.Message}";
                TempData["MessageType"] = "danger";
                return RedirectToAction("DownloadPage");
            }
        }

    }
     
}
  

