using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.ProcedureRepositories.ToplamAnketBilgisi;

namespace SiaAdmin.Application.Features.Queries.StoredProcedure.ToplamAnketBilgisi
{
    public class GetTotalSurveyInformationHandler:IRequestHandler<GetTotalSurveyInformationRequest,GetTotalSurveyInformationResponse>
    {
        private IGetTotalSurveyInformation _getTotalSurveyInformation;
        private IMapper _mapper;
        public GetTotalSurveyInformationHandler(IGetTotalSurveyInformation getTotalSurveyInformation, IMapper mapper)
        {
            _getTotalSurveyInformation = getTotalSurveyInformation;
            _mapper = mapper;
        }

        public async Task<GetTotalSurveyInformationResponse> Handle(GetTotalSurveyInformationRequest request, CancellationToken cancellationToken)
        {
            var list =
               await _getTotalSurveyInformation.GetProcedureListWithDateRange($"{nameof(Enums.StoredProcedure.ToplamAnketBilgisi)}",request.StartDate,request.EndDate);
            var response = _mapper.Map<List<Mapping.Profiles.Procedure.ToplamAnketBilgisi>>(list);
            return new GetTotalSurveyInformationResponse() { data = response };
        }
    }
}
