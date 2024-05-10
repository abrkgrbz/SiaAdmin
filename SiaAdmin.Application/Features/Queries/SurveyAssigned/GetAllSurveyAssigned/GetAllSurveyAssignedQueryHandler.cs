using AutoMapper;
using MediatR;
using SiaAdmin.Application.DTOs.Survey;
using SiaAdmin.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.DTOs.SurveyAssigned;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetAllSurveyAssigned
{
    public class GetAllSurveyAssignedQueryHandler:IRequestHandler<GetAllSurveyAssignedQueryRequest, GetAllSurveyAssignedQueryResponse>
    {
        readonly ISurveyAssignedReadRepository _surveyAssignedReadRepository;
        IMapper _mapper;    
        public GetAllSurveyAssignedQueryHandler(ISurveyAssignedReadRepository surveyAssignedReadRepository, IMapper mapper)
        {
            _surveyAssignedReadRepository = surveyAssignedReadRepository;
            _mapper = mapper;
        }

        public async Task<GetAllSurveyAssignedQueryResponse> Handle(GetAllSurveyAssignedQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _surveyAssignedReadRepository.GetAll(false).Skip(request.start).Take(request.length).ToListAsync();
            var mapSurvey = _mapper.Map<List<ListSurveyAssigned>>(data);
            int countTotal = await _surveyAssignedReadRepository.GetCount(false);
            return new()
            {
                SurveyAssignedList = mapSurvey,
                recordTotal = countTotal,
                recordsFiltered = data.Count
            };

        }
    }
}
