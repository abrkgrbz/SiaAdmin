using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Validators.Survey
{
    public class CreateSurveyValidator:AbstractValidator<CreateSurveyRequest>
    {
        private readonly ISurveyReadRepository _surveyReadRepository;
        public CreateSurveyValidator(ISurveyReadRepository surveyReadRepository)
        {
            _surveyReadRepository = surveyReadRepository;
            RuleFor(x => x.Id).Must(id =>
            {
                bool exists = _surveyReadRepository.IsUniqueSurvey(id);
                return exists;

            }).WithMessage("Bu Proje Kodu kayıtlı!");
            RuleFor(c => c.SurveyText)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Anket Başlığını boş bırakmayınız");

            RuleFor(c => c.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Kodu boş bırakmayınız");
                
            RuleFor(c => c.SurveyDescription)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Açıklamasını boş bırakmayınız");
            RuleFor(c => c.SurveyLink)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Linki'ni boş bırakmayınız");
            RuleFor(c => c.SurveyLinkText)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen 'tıklayınız' ibaresi alanını boş bırakmayınız");
            RuleFor(c => c.SurveyPoints)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Puanı'nı boş bırakmayınız")
                .GreaterThan(1)
                .WithMessage("Proje Puanı '0' dan büyük olmalıdır.");
            //RuleFor(c => c.Mandatory)
            //    .NotNull()
            //    .NotEmpty()
            //    .WithMessage("Lütfen Proje Zorunluluğunu seçiniz");
            RuleFor(c => c.DBAddress)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje SSI Kodu'nu boş bırakmayınız");
            RuleFor(c => c.DPPass)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Şifre bilgisini boş bırakmayınız");

        }

         
    }
}
