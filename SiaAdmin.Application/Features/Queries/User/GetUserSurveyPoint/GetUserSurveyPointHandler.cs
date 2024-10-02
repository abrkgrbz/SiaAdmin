using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserSurveyPoint
{
    public class GetUserSurveyPointHandler:IRequestHandler<GetUserSurveyPointRequest,Response<UserSurveyPointViewModel>>
    {
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;
        public GetUserSurveyPointHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }


        public async Task<Response<UserSurveyPointViewModel>> Handle(GetUserSurveyPointRequest request, CancellationToken cancellationToken)
        {
            var result = _userReadRepository.GetUserSurveyPoints(Guid.Parse(request.UserGUID));
            var mappingProfile = _mapper.Map<UserSurveyPointViewModel>(result);
            return new Response<UserSurveyPointViewModel>(mappingProfile);
        }
    }
}
