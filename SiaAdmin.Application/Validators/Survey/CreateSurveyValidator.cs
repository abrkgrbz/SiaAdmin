using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Validators.Survey
{
    public class CreateSurveyValidator : AbstractValidator<CreateSurveyRequest>
    {
        private readonly ISurveyReadRepository _surveyReadRepository;

        public CreateSurveyValidator(ISurveyReadRepository surveyReadRepository)
        {
            _surveyReadRepository = surveyReadRepository;

            RuleFor(x => x.Id)
                .Must(id => _surveyReadRepository.IsUniqueSurvey(id))
                .WithMessage("Bu Proje Kodu kayıtlı!");

            RuleFor(c => c.SurveyText)
                .NotEmpty()
                .WithMessage("Lütfen Anket Başlığını boş bırakmayınız");

            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Lütfen Proje Kodu boş bırakmayınız");

            RuleFor(c => c.SurveyDescription)
                .NotEmpty()
                .WithMessage("Lütfen Proje Açıklamasını boş bırakmayınız");

            RuleFor(c => c.SurveyLink)
                .NotEmpty()
                .WithMessage("Lütfen Proje Linki'ni boş bırakmayınız");

            RuleFor(c => c.SurveyLinkText)
                .NotEmpty()
                .WithMessage("Lütfen 'tıklayınız' ibaresi alanını boş bırakmayınız");

            RuleFor(c => c.SurveyPoints)
                .NotEmpty()
                .WithMessage("Lütfen Proje Puanı'nı boş bırakmayınız")
                .GreaterThan(1)
                .WithMessage("Proje Puanı '0' dan büyük olmalıdır.");

            RuleFor(c => c.DBAddress)
                .NotEmpty()
                .WithMessage("Lütfen Proje SSI Kodu'nu boş bırakmayınız");

            RuleFor(c => c.DPPass)
                .NotEmpty()
                .WithMessage("Lütfen Proje Şifre bilgisini boş bırakmayınız");

            RuleFor(x => new { x.SurveyLink, x.DBAddress })
                .Must(x => IsVerifySurveyLink(x.SurveyLink, x.DBAddress))
                .WithMessage("Proje linki ile Proje SSI kodu eşleşmemekte lütfen kontrol ediniz!");
        }

        private bool IsVerifySurveyLink(string link, string dbadress)
        {
            Uri uri = new Uri(link);
            string? param = HttpUtility.ParseQueryString(uri.Query).Get("studyname");
            return param == dbadress;
        }
    }
}
