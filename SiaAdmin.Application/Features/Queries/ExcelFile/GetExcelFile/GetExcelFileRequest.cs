using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Enums;

namespace SiaAdmin.Application.Features.Queries.ExcelFile.GetExcelFile
{
    public class GetExcelFileRequest:IRequest<GetExcelFileResponse>
    {
        public string? type { get; set; }
    }
}
