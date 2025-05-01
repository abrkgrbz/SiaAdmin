using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.Excel
{
	public interface IDataTableExcelService
	{
		byte[] ExportToExcel(DataTable dataTable);
		DataTable ImportFromExcel(byte[] excelData);
	}
}
