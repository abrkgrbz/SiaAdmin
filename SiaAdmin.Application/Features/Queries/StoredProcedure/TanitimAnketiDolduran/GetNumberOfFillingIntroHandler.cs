using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.ProcedureRepositories.TanitimAnketiDolduran;

namespace SiaAdmin.Application.Features.Queries.StoredProcedure.TanitimAnketiDolduran
{
    public class GetNumberOfFillingIntroHandler:IRequestHandler<GetNumberOfFillingIntroRequest,GetNumberOfFillingIntroResponse>
    {
        private readonly IGetNumberOfFillingSurveyIntro _getNumberOfFillingSurvey;
        private readonly IMapper _mapper;
        public GetNumberOfFillingIntroHandler(IGetNumberOfFillingSurveyIntro getNumberOfFillingSurvey, IMapper mapper)
        {
            _getNumberOfFillingSurvey = getNumberOfFillingSurvey;
            _mapper = mapper;
        }

        public async Task<GetNumberOfFillingIntroResponse> Handle(GetNumberOfFillingIntroRequest request, CancellationToken cancellationToken)
        {

            var list =
                await _getNumberOfFillingSurvey.GetProcedureListWithDateRange($"{nameof(Enums.StoredProcedure.TanitimAnketiDolduran)}", request.StartDate, request.EndDate);
            var response = _mapper.Map<List<Mapping.Profiles.Procedure.TanitimDolduran>>(list);
            return new GetNumberOfFillingIntroResponse() { data = response };

        }
    }
}
