using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.DataTableExcel
{
	public class DataTableExportExcelHandler<T> : IRequestHandler<DataTableExportExcelRequest<T>, byte[]>
	{
		private IDataTableExcelService _dataTableExcelService;

		public DataTableExportExcelHandler(IDataTableExcelService dataTableExcelService)
		{
			_dataTableExcelService = dataTableExcelService;
		}

	 
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public async Task<byte[]> Handle(DataTableExportExcelRequest<T> request, CancellationToken cancellationToken)
        {
            var dataTable = ConvertToDataTable(request.data);
            var result = _dataTableExcelService.ExportToExcel(dataTable);
            return result;
        }
    }
}
