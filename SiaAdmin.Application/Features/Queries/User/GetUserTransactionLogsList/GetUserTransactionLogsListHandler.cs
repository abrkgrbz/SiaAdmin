using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserTransactionLogsList
{
    public class GetUserTransactionLogsListHandler : IRequestHandler<GetUserTransactionLogsListRequest, Response<List<UserTransactionViewModel>>>
    {
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;

        public GetUserTransactionLogsListHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }


        public async Task<Response<List<UserTransactionViewModel>>> Handle(GetUserTransactionLogsListRequest request, CancellationToken cancellationToken)
        {
            var result = _userReadRepository.GetUserTransactionLogsList(Guid.Parse(request.UserGUID));
            var mappingProfile = _mapper.Map<List<UserTransactionViewModel>>(result);
            return new Response<List<UserTransactionViewModel>>(mappingProfile);
        }
    }
}
