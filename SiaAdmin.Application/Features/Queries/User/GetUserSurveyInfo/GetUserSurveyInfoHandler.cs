using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Features.Queries.User.GetUserSurveyPoint;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserSurveyInfo
{
    public class GetUserSurveyInfoHandler:IRequestHandler<GetUserSurveyInfoRequest,Response<List<GetUserSurveyInfoViewModel>>>
    {
        private readonly IUserReadRepository _userReadRepository;
        private IMapper _mapper;

        public GetUserSurveyInfoHandler(IMapper mapper, IUserReadRepository userReadRepository)
        {
            _mapper = mapper;
            _userReadRepository = userReadRepository;
        }

        public async Task<Response<List<GetUserSurveyInfoViewModel>>> Handle(GetUserSurveyInfoRequest request, CancellationToken cancellationToken)
        {
            var result = _userReadRepository.GetUserSurveyInfo(Guid.Parse(request.UserGUID));
            var mappingProfile = _mapper.Map<List<GetUserSurveyInfoViewModel>>(result);
            return new Response<List<GetUserSurveyInfoViewModel>>(mappingProfile);
        }
    }
}
