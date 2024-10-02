using AutoMapper;
using MediatR;
using SiaAdmin.Application.Features.Queries.User.GetListLastSeenAdet;
using SiaAdmin.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetListLastSeenSaat
{
	public class GetListLastSeenSaatHandler:IRequestHandler<GetListLastSeenSaatRequest,GetListLastSeenSaatResponse>
	{
		private readonly IUserReadRepository _userReadRepository;
		private readonly IMapper _mapper;

		public GetListLastSeenSaatHandler(IUserReadRepository userReadRepository, IMapper mapper)
		{
			_userReadRepository = userReadRepository;
			_mapper = mapper;
		}

		public async Task<GetListLastSeenSaatResponse> Handle(GetListLastSeenSaatRequest request, CancellationToken cancellationToken)
		{
			var result = await _userReadRepository.GetListLastSeenSaat();
			var mapping = _mapper.Map<List<GetListLastSeenSaatViewModel>>(result);
			return new GetListLastSeenSaatResponse() {data=mapping};
		}
	}
}
