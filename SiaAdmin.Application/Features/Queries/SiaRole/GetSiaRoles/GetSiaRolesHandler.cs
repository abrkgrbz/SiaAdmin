using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.SiaRole.GetSiaRoles
{
    public class GetSiaRolesHandler : IRequestHandler<GetSiaRolesRequest, GetSiaRolesResponse>
    {
        private ISiaRoleReadRepository _siaRoleReadRepository;
        private IMapper _mapper;

        public GetSiaRolesHandler(ISiaRoleReadRepository siaRoleReadRepository, IMapper mapper)
        {
            _siaRoleReadRepository = siaRoleReadRepository;
            _mapper = mapper;
        }

        public async Task<GetSiaRolesResponse> Handle(GetSiaRolesRequest request, CancellationToken cancellationToken)
        {
            var roleList = await _siaRoleReadRepository.GetAll(false).ToListAsync(cancellationToken: cancellationToken);
            var mappingProfile = _mapper.Map<List<SiaRolesViewModel>>(roleList);
            return new GetSiaRolesResponse() { SiaRolesViewModels = mappingProfile };
        }
    }
}
