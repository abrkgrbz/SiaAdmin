using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetByGuidSiaUser
{
    public class GetByGuidSiaUserHandler : IRequestHandler<GetByGuidSiaUserRequest, GetByGuidSiaUserResponse>
    {
        private readonly IUserReadRepository _usersReadRepository;
        private readonly ISurveyLogReadRepository _surveyLogReadRepository;
        readonly IMapper _mapper;
        public GetByGuidSiaUserHandler(  ISurveyLogReadRepository surveyLogReadRepository, IMapper mapper, IUserReadRepository usersReadRepository)
        {
             
            _surveyLogReadRepository = surveyLogReadRepository;
            _mapper = mapper;
            _usersReadRepository = usersReadRepository;
        }

        public async Task<GetByGuidSiaUserResponse> Handle(GetByGuidSiaUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _usersReadRepository.GetWhere(x => x.SurveyUserGuid == request.SurveyUserGuid,false).FirstOrDefaultAsync();
            if (user==null)
                throw new ApiException("Kullanıcı bulunamadı");
            var surveyLogDetail = await _surveyLogReadRepository.GetWhere(x => x.SurveyUserGuid == request.SurveyUserGuid,false).ToListAsync();
            var mappingProfile = _mapper.Map<List<GetByGuidSiaUserViewModel>>(surveyLogDetail);
            return new GetByGuidSiaUserResponse() { data = mappingProfile };
        }
    }
}
