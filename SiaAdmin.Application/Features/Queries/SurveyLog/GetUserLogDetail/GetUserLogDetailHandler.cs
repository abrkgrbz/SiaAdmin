using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SurveyLog.GetUserLogDetail
{
    public class GetUserLogDetailHandler:IRequestHandler<GetUserLogdDetailRequest,List<GetUserLogDetailViewModel>>
    {
        private ISurveyLogReadRepository _surveyLogReadRepository;
        private IMapper _mapper;

        public GetUserLogDetailHandler(IMapper mapper, ISurveyLogReadRepository surveyLogReadRepository)
        {
            _mapper = mapper;
            _surveyLogReadRepository = surveyLogReadRepository;
        }

        public async Task<List<GetUserLogDetailViewModel>> Handle(GetUserLogdDetailRequest request, CancellationToken cancellationToken)
        {
            var data =await _surveyLogReadRepository.GetWhere(x => x.SurveyUserGuid == Guid.Parse(request.SurveyUserGUID))
                .ToListAsync();
            var mappingProfile = _mapper.Map<GetUserLogDetailViewModel>(data);
            return new List<GetUserLogDetailViewModel>() { mappingProfile };
        }
    }
}
