using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLogByUser;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Validators.SurveyLog
{
    public class CreateSurveyLogByUserRequestValidator:AbstractValidator<CreateSurveyLogByUserRequest>
    {
        private readonly IIncentiveReadRepository _incentiveReadRepository;
        public CreateSurveyLogByUserRequestValidator(IIncentiveReadRepository incentiveReadRepository)
        {
            _incentiveReadRepository = incentiveReadRepository;
            RuleFor(x => x.IncentiveId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Hediye bilgisi boş olamaz");

            RuleFor(x => x.IncentiveId)
                .Must(IsExistValidate)
                .WithMessage("Yanlış Id bilgisi");
        }

        bool IsExistValidate(int incentiveId)
        {
            bool isExist = _incentiveReadRepository.GetAll(false).Any(x => x.Id.Equals(incentiveId));
            return isExist;
        }

    }
}
