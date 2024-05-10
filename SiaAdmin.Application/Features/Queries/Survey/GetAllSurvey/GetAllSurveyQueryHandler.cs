using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.DTOs.Survey;
using SiaAdmin.Application.Features.Commands.BlockList.CreateBlockList;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Survey.GetAllSurvey
{
    public class GetAllSurveyQueryHandler : IRequestHandler<GetAllSurveyQueryRequest, GetAllSurveyQueryResponse>
    {
        readonly ISurveyReadRepository _surveyReadRepository;
        private IMapper _mapper;
        public GetAllSurveyQueryHandler(ISurveyReadRepository surveyReadRepository, IMapper mapper)
        {
            _surveyReadRepository = surveyReadRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSurveyQueryResponse> Handle(GetAllSurveyQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _surveyReadRepository.GetAll(false).Skip(request.start).Take(request.length).ToListAsync();
            var mapSurvey = _mapper.Map<List<ListSurvey>>(data);
            int countTotal = await _surveyReadRepository.GetCount(false);
             
            return new()
            {
                SurveyList = mapSurvey,
                recordTotal = countTotal,
                recordsFiltered = mapSurvey.Count
            };

        }
    }
}
