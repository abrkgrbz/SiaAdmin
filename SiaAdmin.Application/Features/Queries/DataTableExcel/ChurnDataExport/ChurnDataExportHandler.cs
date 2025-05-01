using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.StaticMethods;

namespace SiaAdmin.Application.Features.Queries.DataTableExcel.ChurnDataExport
{
    public class ChurnDataExportHandler : IRequestHandler<ChurnDataExportRequest, byte[]>
    {
        private IUserReadRepository _userReadRepository;
        private IDataTableExcelService _dataTableExcelService;
        public ChurnDataExportHandler(IUserReadRepository userReadRepository, IDataTableExcelService dataTableExcelService)
        {
            _userReadRepository = userReadRepository;
            _dataTableExcelService = dataTableExcelService;
        }

        public async Task<byte[]> Handle(ChurnDataExportRequest request, CancellationToken cancellationToken)
        {
            var data = await _userReadRepository.GetAllChurnData();
            var dataTable = ConvertToDataTable.Convert(data);
            var result = _dataTableExcelService.ExportToExcel(dataTable);
            return result;
        }
    }
}
