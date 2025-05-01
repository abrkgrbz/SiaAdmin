using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.DataTableExcel
{
	public class DataTableExportExcelRequest<T> : IRequest<byte[]>
	{
		public List<T> data { get; set; }
	}
}
