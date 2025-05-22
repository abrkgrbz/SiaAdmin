using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.DTOs.Excel;
using SiaAdmin.Application.Enums;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Interfaces.QueryBuilder;
using SiaAdmin.Domain.Entities.ReportModel;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http.Extensions;

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

        /// <summary>
        /// Şablon Excel dosyasından yeni Excel raporu oluşturur
        /// </summary>
        /// <param name="data">Excel'e yazılacak veri</param>
        /// <param name="report">Rapor bilgileri</param>
        /// <param name="templateFileName">Kullanılacak şablon dosya adı</param>
        /// <returns>Excel dosyasının byte array'i</returns>
        public async Task<byte[]> GenerateExcelFromTemplateAsync(DataTable data, Report report, string templateFileName)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Veri tablosu null olamaz.");

            if (report == null)
                throw new ArgumentNullException(nameof(report), "Rapor bilgisi null olamaz.");

            if (string.IsNullOrEmpty(templateFileName))
                throw new ArgumentException("Şablon dosya adı boş olamaz.", nameof(templateFileName));

            try
            {
                // Şablon dosyasının yolu
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", templateFileName);

                if (!File.Exists(templatePath))
                {
                    throw new FileNotFoundException($"Excel şablonu bulunamadı: {templateFileName}", templatePath);
                }

                // Şablon dosyasını hafızaya kopyala
                byte[] templateBytes = await File.ReadAllBytesAsync(templatePath);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await memoryStream.WriteAsync(templateBytes, 0, templateBytes.Length);

                    using (SpreadsheetDocument document = SpreadsheetDocument.Open(memoryStream, true))
                    {
                        WorkbookPart workbookPart = document.WorkbookPart;

                        // Belirtilen çalışma sayfasını bul
                        Sheet targetSheet = workbookPart.Workbook.Descendants<Sheet>()
                            .FirstOrDefault(s => s.Name == report.SheetName);

                        if (targetSheet == null)
                        {
                            throw new InvalidOperationException($"Şablonda '{report.SheetName}' adında çalışma sayfası bulunamadı.");
                        }

                        // Worksheet'i al
                        WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(targetSheet.Id);

                        // Mevcut verileri temizle ve yeni verileri ekle
                        await PopulateWorksheetWithDataAsync(worksheetPart, data, report, true);
 

                        // Değişiklikleri kaydet
                        workbookPart.Workbook.Save();
                    }

                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Şablondan Excel oluşturulurken hata oluştu: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sıfırdan yeni Excel raporu oluşturur
        /// </summary>
        /// <param name="data">Excel'e yazılacak veri</param>
        /// <param name="report">Rapor bilgileri</param>
        /// <returns>Excel dosyasının byte array'i</returns>
        public async Task<byte[]> GenerateExcelAsync(DataTable data, Report report)
        {
             
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Veri tablosu null olamaz.");

            if (report == null)
                throw new ArgumentNullException(nameof(report), "Rapor bilgisi null olamaz.");

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Yeni Excel dosyası oluştur
                    using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                    {
                        // Workbook bileşeni oluştur
                        WorkbookPart workbookPart = document.AddWorkbookPart();
                        workbookPart.Workbook = new Workbook();

                        // Styles bileşeni oluştur
                        WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                        stylesPart.Stylesheet = CreateStylesheet();
                        stylesPart.Stylesheet.Save();

                        // Sheets bileşeni oluştur
                        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                        // WorksheetPart oluştur
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet(new SheetData());

                        // Sheet referansı oluştur
                        Sheet sheet = new Sheet()
                        {
                            Id = workbookPart.GetIdOfPart(worksheetPart),
                            SheetId = 1,
                            Name = report.SheetName ?? "Rapor"
                        };
                        sheets.Append(sheet);

                        // Veri ekle
                        await PopulateWorksheetWithDataAsync(worksheetPart, data, report, false);

                        // Sütun genişliklerini ayarla
                        AddColumnWidths(worksheetPart, data.Columns.Count);

                        // AutoFilter ekle
                        AddAutoFilter(worksheetPart, data.Columns.Count, GetDataStartRow(report));
                          
                        // Değişiklikleri kaydet
                        workbookPart.Workbook.Save();
                    }

                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Excel oluşturulurken hata oluştu: {ex.Message}", ex);
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Worksheet'e veri ekler
        /// </summary>
        private async Task PopulateWorksheetWithDataAsync(WorksheetPart worksheetPart, DataTable data, Report report, bool clearExisting)
        {
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            if (clearExisting)
            {
                sheetData.RemoveAllChildren();
            }

            uint currentRowIndex = 1;

            // Rapor başlığı ekle (varsa)
            if (!string.IsNullOrEmpty(report.Title))
            {
                Row titleRow = new Row() { RowIndex = currentRowIndex };
                sheetData.Append(titleRow);

                Cell titleCell = new Cell()
                {
                    DataType = CellValues.String,
                    CellReference = "A" + currentRowIndex,
                    CellValue = new CellValue(report.Title),
                    StyleIndex = 4  // Başlık stili
                };
                titleRow.Append(titleCell);

                currentRowIndex++;

                // Boş satır ekle
                Row emptyRow = new Row() { RowIndex = currentRowIndex };
                sheetData.Append(emptyRow);
                currentRowIndex++;
            }

            // Sütun başlıkları ekle
            Row headerRow = new Row() { RowIndex = currentRowIndex };
            sheetData.Append(headerRow);

            for (int i = 0; i < data.Columns.Count; i++)
            {
                Cell headerCell = new Cell()
                {
                    DataType = CellValues.String,
                    CellReference = GetExcelColumnName(i + 1) + currentRowIndex,
                    CellValue = new CellValue(GetColumnDisplayName(data.Columns[i].ColumnName, report)),
                    StyleIndex = 1  // Header stili
                };
                headerRow.Append(headerCell);
            }

            currentRowIndex++;

            // Veri satırlarını ekle
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Row dataRow = new Row() { RowIndex = currentRowIndex };
                sheetData.Append(dataRow);

                for (int j = 0; j < data.Columns.Count; j++)
                {
                    Cell cell = CreateDataCell(data.Rows[i][j], GetExcelColumnName(j + 1) + currentRowIndex);
                    dataRow.Append(cell);
                }

                currentRowIndex++;
            }

            await Task.CompletedTask; // Async pattern için
        }

        /// <summary>
        /// Veri hücresi oluşturur
        /// </summary>
        private Cell CreateDataCell(object value, string cellReference)
        {
            Cell cell = new Cell() { CellReference = cellReference };

            if (value == null || value == DBNull.Value)
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(string.Empty);
            }
            else if (value is DateTime dateValue)
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(dateValue.ToString("dd/MM/yyyy HH:mm:ss"));
                cell.StyleIndex = 2; // Tarih stili
            }
            else if (value is int || value is long || value is double || value is decimal || value is float)
            {
                cell.DataType = CellValues.Number;
                cell.CellValue = new CellValue(value.ToString());
                cell.StyleIndex = 3; // Sayı stili
            }
            else if (value is bool boolValue)
            {
                cell.DataType = CellValues.Boolean;
                cell.CellValue = new CellValue(boolValue ? "1" : "0");
            }
            else
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(value.ToString());
            }

            return cell;
        }

        /// <summary>
        /// Excel sütun adını oluşturur (A, B, C, ..., AA, AB, ...)
        /// </summary>
        private string GetExcelColumnName(int columnIndex)
        {
            string columnName = string.Empty;

            while (columnIndex > 0)
            {
                int remainder = (columnIndex - 1) % 26;
                columnName = (char)('A' + remainder) + columnName;
                columnIndex = (columnIndex - 1) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// Sütun görüntü adını alır
        /// </summary>
        private string GetColumnDisplayName(string columnName, Report report)
        { 
            if (report.ColumnMappings != null && report.ColumnMappings.TryGetValue(columnName, out string mappedName))
            {
                return mappedName;
            }
             
            return columnName;
        }

        /// <summary>
        /// Veri başlangıç satır numarasını döndürür
        /// </summary>
        private uint GetDataStartRow(Report report)
        {
            return string.IsNullOrEmpty(report.Title) ? 1u : 3u;
        }

        /// <summary>
        /// Sütun genişliklerini ayarlar
        /// </summary>
        private void AddColumnWidths(WorksheetPart worksheetPart, int columnCount)
        {
            Columns columns = new Columns();

            for (int i = 1; i <= columnCount; i++)
            {
                Column column = new Column()
                {
                    Min = (uint)i,
                    Max = (uint)i,
                    Width = 20,
                    CustomWidth = true
                };
                columns.Append(column);
            }

            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            worksheetPart.Worksheet.InsertBefore(columns, sheetData);
        }

        /// <summary>
        /// AutoFilter ekler
        /// </summary>
        private void AddAutoFilter(WorksheetPart worksheetPart, int columnCount, uint headerRowIndex)
        {
            AutoFilter autoFilter = new AutoFilter()
            {
                Reference = $"A{headerRowIndex}:{GetExcelColumnName(columnCount)}{headerRowIndex}"
            };

            worksheetPart.Worksheet.Append(autoFilter);
        }

   
        /// <summary>
        /// Excel stilleri oluşturur
        /// </summary>
        private Stylesheet CreateStylesheet()
        {
            Stylesheet stylesheet = new Stylesheet();

            // Fonts
            Fonts fonts = new Fonts();
            fonts.Append(new Font(new FontSize() { Val = 11 }, new FontName() { Val = "Calibri" })); // 0 - Normal
            fonts.Append(new Font(new Bold(), new FontSize() { Val = 11 }, new Color() { Rgb = new HexBinaryValue() { Value = "FFFFFF" } }, new FontName() { Val = "Calibri" })); // 1 - Bold Header
            fonts.Append(new Font(new Bold(), new FontSize() { Val = 16 }, new Color() { Rgb = new HexBinaryValue() { Value = "2F4F4F" } }, new FontName() { Val = "Calibri" })); // 2 - Title
            fonts.Count = (uint)fonts.ChildElements.Count;

            // Fills
            Fills fills = new Fills();
            fills.Append(new Fill(new PatternFill() { PatternType = PatternValues.None })); // 0 - Required
            fills.Append(new Fill(new PatternFill() { PatternType = PatternValues.Gray125 })); // 1 - Required
            fills.Append(new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "4472C4" } }) { PatternType = PatternValues.Solid })); // 2 - Header Blue
            fills.Append(new Fill(new PatternFill(new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "F2F2F2" } }) { PatternType = PatternValues.Solid })); // 3 - Light Gray
            fills.Count = (uint)fills.ChildElements.Count;

            // Borders
            Borders borders = new Borders();
            borders.Append(new Border()); // 0 - No border
            borders.Append(new Border( // 1 - Thin border
                new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }
            ));
            borders.Count = (uint)borders.ChildElements.Count;

            // Number Formats
            NumberingFormats numberingFormats = new NumberingFormats();
            numberingFormats.Append(new NumberingFormat() { NumberFormatId = 165, FormatCode = "dd/mm/yyyy hh:mm:ss" });
            numberingFormats.Count = 1;

            // Cell Formats
            CellFormats cellFormats = new CellFormats();
            cellFormats.Append(new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 }); // 0 - Normal
            cellFormats.Append(new CellFormat() { FontId = 1, FillId = 2, BorderId = 1, ApplyFont = true, ApplyFill = true, ApplyBorder = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } }); // 1 - Header
            cellFormats.Append(new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, NumberFormatId = 165, ApplyNumberFormat = true }); // 2 - Date
            cellFormats.Append(new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, NumberFormatId = 2, ApplyNumberFormat = true }); // 3 - Number
            cellFormats.Append(new CellFormat() { FontId = 2, FillId = 3, BorderId = 1, ApplyFont = true, ApplyFill = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } }); // 4 - Title
            cellFormats.Count = (uint)cellFormats.ChildElements.Count;

            stylesheet.Append(numberingFormats);
            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders);
            stylesheet.Append(cellFormats);

            return stylesheet;
        }

        #endregion
    }
}
