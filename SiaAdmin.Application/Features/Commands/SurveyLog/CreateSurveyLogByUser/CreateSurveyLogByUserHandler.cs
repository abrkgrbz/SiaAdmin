using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLogByUser
{
    public class CreateSurveyLogByUserHandler:IRequestHandler<CreateSurveyLogByUserRequest,Response<int>>
    {
        private ISurveyLogWriteRepository _surveyLogWriteRepository;

        public CreateSurveyLogByUserHandler(ISurveyLogWriteRepository surveyLogWriteRepository)
        {
            _surveyLogWriteRepository = surveyLogWriteRepository;
        }

        public async Task<Response<int>> Handle(CreateSurveyLogByUserRequest request, CancellationToken cancellationToken)
        {
            var response =
               await _surveyLogWriteRepository.CreateSurveyLogByUserIncentive(request.IncentiveId, request.InternalGuid);
            if (response==1)
            {
                return new Response<int>(response, "Hediyeniz sisteme yüklendi");
            }

            return new Response<int>(response, "Hediye Yükleme sırasında bir hata meydana geldi");
         
        }
    }
}
