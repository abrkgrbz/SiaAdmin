using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey
{
    public class GetDataTableSurveyQueryHandler:IRequestHandler<GetDataTableSurveyQueryRequest,GetDataTableSurveyQueryResponse>
    {
        readonly ISurveyReadRepository _surveyReadRepository;

        public GetDataTableSurveyQueryHandler(ISurveyReadRepository surveyReadRepository)
        {
            _surveyReadRepository = surveyReadRepository;
        }

        public async Task<GetDataTableSurveyQueryResponse> Handle(GetDataTableSurveyQueryRequest request, CancellationToken cancellationToken)
        {
            var surveyList = _surveyReadRepository.GetAll(false);
            int recordsFiltered=0, recordTotal=0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                surveyList = surveyList.Where(x => x.SurveyDescription.ToLower().Contains(request.searchValue.ToLower())
                                                   || x.SurveyText.ToLower().Contains(request.searchValue.ToLower())
                                                   || x.SurveyLink.ToLower().Contains(request.searchValue.ToLower())
                                                   || x.Id.ToString().Equals(request.searchValue));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                surveyList =  await _surveyReadRepository.OrderByField(surveyList, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = surveyList.Count();
                recordTotal = surveyList.Count();
            }

            var surveys =await surveyList.Skip(request.Start).Take(request.Length).ToListAsync();
            if (surveys == null) throw new Exception("Anket bulunamadı");
            return new GetDataTableSurveyQueryResponse()
            {
                recordTotal = recordTotal,
                recordsFiltered = recordsFiltered,
                data = surveys 
            };
        }
    }
}
