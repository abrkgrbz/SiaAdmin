using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SiaAdmin.Application.Enums;
using SiaAdmin.Application.Features.Commands.SurveyAssigned.CreateSurveyAssigned;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Validators.SurveyAssigned
{
    public class CreateSurveyAssignedValidator:AbstractValidator<CreateSurveyAssignedRequest>
    { 
       private readonly IExcelService _excelService;
        public CreateSurveyAssignedValidator(IExcelService excelService)
        {
            _excelService = excelService;

            RuleFor(c => c.SurveyId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen Proje Kodu Seçiniz");
         
            RuleFor(c => c.SurveyPoints)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen Proje Atamada ki Puan alanını boş bırakmayınız")
                .GreaterThan(1)
                .WithMessage("Lütfen 1 den büyük bir değer giriniz");

            RuleFor(c => c.ExcelFile)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen Excel şablonu seçiniz")
                .Must(IsTrueExcelFile)
                .WithMessage("Yanlış Şablon Gönderimi");
        }

        private bool IsTrueExcelFile(IFormFile file)
        {
            string excelFileName = _excelService.readExcel(file).TableName;
            if (excelFileName.Equals(nameof(ExcelTable.InternalGUID)) 
                || excelFileName.Equals(nameof(ExcelTable.SurveyUserGUID)))
            {
                return true;
            }

            return false;
        }
    }
}
