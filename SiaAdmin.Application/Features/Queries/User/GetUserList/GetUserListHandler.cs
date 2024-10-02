using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.User.GetUserList
{
    public class GetUserListHandler : IRequestHandler<GetUserListRequest, GetUserListResponse>
    {
        private ISiaUserReadRepository _siaUserReadRepository;  
        private IMapper _mapper;
        public GetUserListHandler(ISiaUserReadRepository siaUserReadRepository, IMapper mapper )
        {
            _siaUserReadRepository = siaUserReadRepository;
            _mapper = mapper; 
        }


        public async Task<GetUserListResponse> Handle(GetUserListRequest request, CancellationToken cancellationToken)
        {
            var userList =await _siaUserReadRepository.GetAll(false).ToListAsync();
            var mapping = _mapper.Map<List<UserListViewModel>>(userList);
            return new GetUserListResponse() { UserListViewModels = mapping };
        }
    }
}
