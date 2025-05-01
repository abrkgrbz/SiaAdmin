using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.DataTableExcel.ChurnDataExport
{
    public class ChurnDataExportRequest : IRequest<byte[]>
    {
    }
}
