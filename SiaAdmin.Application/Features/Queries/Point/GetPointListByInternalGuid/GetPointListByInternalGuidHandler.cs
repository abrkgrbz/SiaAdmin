using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Point.GetPointListByInternalGuid
{
    public class GetPointListByInternalGuidHandler:IRequestHandler<GetPointListByInternalGuidRequest,GetPointListByInternalGuidResponse>
    {
        private readonly ISurveyLogReadRepository _surveyLogReadRepository;
        private readonly IMapper _mapper;
        public GetPointListByInternalGuidHandler(ISurveyLogReadRepository surveyLogReadRepository, IMapper mapper)
        {
            _surveyLogReadRepository = surveyLogReadRepository;
            _mapper = mapper;
        }

        public async Task<GetPointListByInternalGuidResponse> Handle(GetPointListByInternalGuidRequest request, CancellationToken cancellationToken)
        {
            var parsedGuid = Guid.Parse(request.SurveyUserGUID);
            var data = await _surveyLogReadRepository.GetWhere(x=>x.SurveyUserGuid==parsedGuid,false).ToListAsync();
            if (data==null)
            {
                throw new ApiException("Data bulunamadı");
            }
            var mappingProfile = _mapper.Map<List<GetPointListViewModel>>(data);
             
            return new GetPointListByInternalGuidResponse() { GetPointListViewModels = mappingProfile,ResponseCode = (int)HttpStatusCode.Accepted};
        }
    }
}
