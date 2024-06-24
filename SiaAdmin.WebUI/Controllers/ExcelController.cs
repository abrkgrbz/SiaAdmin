using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Queries.ExcelFile.GetExcelFile;

namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]
    public class ExcelController : BaseController
    {
        public async Task<FileResult> FileDownload(GetExcelFileRequest request)
        {
            var response = await Mediator.Send(request);
            return File(response.excelFile, "application/octet-stream", response.excelFileName);
        }
    }
}
