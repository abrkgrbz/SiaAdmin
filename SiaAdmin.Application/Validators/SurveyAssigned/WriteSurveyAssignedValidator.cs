using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Validators.SurveyAssigned
{
    public class WriteSurveyAssignedValidator:AbstractValidator<Domain.Entities.Models.SurveyAssigned>
    { 
        private readonly ISurveyAssignedWriteRepository _surveyAssignedWriteRepository;
        public WriteSurveyAssignedValidator(ISurveyAssignedWriteRepository surveyAssignedWriteRepository)
        {
            _surveyAssignedWriteRepository = surveyAssignedWriteRepository;


            RuleFor(m => new { m.SurveyId, m.InternalGuid })
                .Must(x=>IsDuplicatedGuid(x.SurveyId,x.InternalGuid))
                .WithMessage("Mükerrer kayıt bulunmakta");
        }

        private bool IsDuplicatedGuid(int surveyId, Guid internalGuid)
        {
            return _surveyAssignedWriteRepository.IsDuplicatedGuid(surveyId, internalGuid);
        }
    }
}
