using MediatR;
using SiaAdmin.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetSurveyUserList
{
    public class GetSurveyUserListHandler:IRequestHandler<GetSurveyUserListRequest, Response<List<GetSurveyUserListViewModel>>>
    {
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;
        public GetSurveyUserListHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }


        public async Task<Response<List<GetSurveyUserListViewModel>>> Handle(GetSurveyUserListRequest request, CancellationToken cancellationToken)
        {
            var result = _userReadRepository.GetUserSurveyList(Guid.Parse(request.UserGUID));
            var mappingProfile = _mapper.Map<List<GetSurveyUserListViewModel>>(result);
            return new Response<List<GetSurveyUserListViewModel>>(mappingProfile);
        }
    }
}
 