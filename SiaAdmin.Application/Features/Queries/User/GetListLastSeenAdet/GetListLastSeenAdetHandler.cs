using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.User.GetListLastSeenAdet
{
	public class GetListLastSeenAdetHandler:IRequestHandler<GetListLastSeenAdetRequest,GetListLastSeenAdetResponse>
	{
		private readonly IUserReadRepository _userReadRepository;
		private readonly IMapper _mapper;

		public GetListLastSeenAdetHandler(IUserReadRepository userReadRepository, IMapper mapper)
		{
			_userReadRepository = userReadRepository;
			_mapper = mapper;
		}

		public async Task<GetListLastSeenAdetResponse> Handle(GetListLastSeenAdetRequest request, CancellationToken cancellationToken)
		{
			var result = await _userReadRepository.GetListLastSeenAdet();
			var mapping = _mapper.Map<List<GetListLastSeenViewModel>>(result);
			return new GetListLastSeenAdetResponse() { data = mapping };
			
		}
	}
}
