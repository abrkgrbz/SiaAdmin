using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.Survey.CloseSurvey
{
    public class CloseSurveyHandler:IRequestHandler<CloseSurveyRequest,Response<bool>>
    {
        private ISurveyWriteRepository _surveyWriteRepository;
        private ISurveyReadRepository _surveyReadRepository;

        public CloseSurveyHandler(ISurveyWriteRepository surveyWriteRepository, ISurveyReadRepository surveyReadRepository)
        {
            _surveyWriteRepository = surveyWriteRepository;
            _surveyReadRepository = surveyReadRepository;
        }

        public async Task<Response<bool>> Handle(CloseSurveyRequest request, CancellationToken cancellationToken)
        {
            var data = await _surveyReadRepository.GetByIdAsync(request.SurveyId, false);
            data.SurveyActive = 0;
            var result = _surveyWriteRepository.Update(data);
            if (result)
            {
                return new Response<bool>(true, "Proje kapatıldı");
            }

            throw new ApiException("Proje kapatma işlemi başarısız");
        }
    }
}
