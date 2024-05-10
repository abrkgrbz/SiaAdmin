using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Validators.Survey
{
    public class CreateSurveyValidator:AbstractValidator<CreateSurveyRequest>
    {
        private ISurveyReadRepository _surveyReadRepository;
        public CreateSurveyValidator(ISurveyReadRepository surveyReadRepository)
        {
            _surveyReadRepository = surveyReadRepository;
            RuleFor(c => c.createSurvey.SurveyText)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Anket Başlığını boş bırakmayınız");

            RuleFor(c => c.createSurvey.SurveyId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Kodu boş bırakmayınız")
                .MustAsync(IsUniqueSurvey).WithMessage("Bu Proje Kodu mevcut!");
            RuleFor(c => c.createSurvey.SurveyDescription)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Açıklamasını boş bırakmayınız");
            RuleFor(c => c.createSurvey.SurveyLink)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Linki'ni boş bırakmayınız");
            RuleFor(c => c.createSurvey.SurveyLinkText)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen 'tıklayınız' ibaresi alanını boş bırakmayınız");
            RuleFor(c => c.createSurvey.SurveyPoints)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Puanı'nı boş bırakmayınız")
                .GreaterThan(1)
                .WithMessage("Proje Puanı '0' dan büyük olmalıdır.");
            RuleFor(c => c.createSurvey.Mandatory)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Zorunluluğunu seçiniz");
            RuleFor(c => c.createSurvey.DBAddress)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje SSI Kodu'nu boş bırakmayınız");
            RuleFor(c => c.createSurvey.DPPass)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen Proje Şifre bilgisini boş bırakmayınız");

        }

        private async Task<bool> IsUniqueSurvey(int surveyId, CancellationToken cancellationToken)
        {
            return await _surveyReadRepository.IsUniqueSurvey(surveyId);

        }
    }
}
