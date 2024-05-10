using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SiaAdmin.Application.Enums;
using SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLog;
using SiaAdmin.Application.Interfaces.Excel;

namespace SiaAdmin.Application.Validators.SurveyLog
{
    public class CreateSurveyLogValidator:AbstractValidator<CreateSurveyLogRequest>
    {
        private IExcelService _excelService;
        public CreateSurveyLogValidator(IExcelService excelService)
        {
            _excelService = excelService;
            RuleFor(x => x.ExcelFile)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Puan Yükleme Şablonunu seçiniz")
                .Must(IsTrueExcelFile)
                .WithMessage("Yanlış Şablon Gönderimi");
        }

        private bool IsTrueExcelFile(IFormFile file)
        {
            string excelFileName = _excelService.readExcel(file).TableName;
            if (excelFileName.Equals(nameof(ExcelTable.Point)))
            {
                return true;
            }

            return false;
        }


    }
}
