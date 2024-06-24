using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Point.GetPointListBySurveyId
{
    public class GetPointListBySurveyIdHandler:IRequestHandler<GetPointListBySurveyIdRequest,GetPointListBySurveyIdResponse>
    {
        private ISurveyLogReadRepository _surveyLogReadRepository;
        readonly IMapper _mapper;
        public GetPointListBySurveyIdHandler(ISurveyLogReadRepository surveyLogReadRepository, IMapper mapper)
        {
            _surveyLogReadRepository = surveyLogReadRepository;
            _mapper = mapper;
        }

        public async Task<GetPointListBySurveyIdResponse> Handle(GetPointListBySurveyIdRequest request, CancellationToken cancellationToken)
        {
            var list = _surveyLogReadRepository.GetWhere(x => x.SurveyId == request.SurveyId).ToList();
            var mappingProfile = _mapper.Map<List<GetPointListBySurveyIdViewModel>>(list);
            return new GetPointListBySurveyIdResponse()
            {
                GetPointListBySurveyIdViewModels = mappingProfile
            }; 
        }
    }
}
