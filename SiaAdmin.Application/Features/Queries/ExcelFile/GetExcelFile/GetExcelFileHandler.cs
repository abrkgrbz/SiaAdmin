using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Interfaces.Excel;

namespace SiaAdmin.Application.Features.Queries.ExcelFile.GetExcelFile
{
    public class GetExcelFileHandler : IRequestHandler<GetExcelFileRequest, GetExcelFileResponse>
    {
        private IExcelService _excelService;

        public GetExcelFileHandler(IExcelService excelService)
        {
            _excelService = excelService;
        }

        public async Task<GetExcelFileResponse> Handle(GetExcelFileRequest request, CancellationToken cancellationToken)
        {
            var file = await _excelService.downloadExcel(request.type);
            return new() { excelFile = file , excelFileName = request.type+".xlsx"};
        }
    }
}
