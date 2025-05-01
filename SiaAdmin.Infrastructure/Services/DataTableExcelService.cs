using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SiaAdmin.Application.Interfaces.Excel;

namespace SiaAdmin.Infrastructure.Services
{
    public class DataTableExcelService:IDataTableExcelService
    {
	    public byte[] ExportToExcel(DataTable dataTable)
	    {
			using (var workbook = new XLWorkbook())
			{
				var worksheet = workbook.Worksheets.Add(dataTable, "Sheet1");
				using (var stream = new MemoryStream())
				{
					workbook.SaveAs(stream);
					return stream.ToArray();
				}
			}
		}

	    public DataTable ImportFromExcel(byte[] excelData)
	    {
		    throw new NotImplementedException();
		}
    }
}
