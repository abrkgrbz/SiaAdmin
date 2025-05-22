using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Survey.GetAllSurveyData
{
    public class GetAllSurveyDataHandler:IRequestHandler<GetAllSurveyDataRequest,GetAllSurveyDataResponse>
    {
        private readonly ISurveyReadRepository _surveyRepository;
        private readonly IMapper _mapper;
        public GetAllSurveyDataHandler(ISurveyReadRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSurveyDataResponse> Handle(GetAllSurveyDataRequest request, CancellationToken cancellationToken)
        {
            var data = _surveyRepository.GetAll(false).ToList();
            var mapping = _mapper.Map<List<GetAllSurveyModel>>(data);
            return new GetAllSurveyDataResponse() { GetAllSurveyModels = mapping };
        }
    }
}
