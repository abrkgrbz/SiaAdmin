using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokenList
{
    public class GetUserDeviceTokenListHandler:IRequestHandler<GetUserDeviceTokenListRequest,Response<List<DeviceTokenList>>>
    {
        private IDeviceRegistrationsReadRepository _deviceRegistrationReadRepository;
        private IMapper _mapper;

        public GetUserDeviceTokenListHandler(IDeviceRegistrationsReadRepository deviceRegistrationReadRepository, IMapper mapper)
        {
            _deviceRegistrationReadRepository = deviceRegistrationReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<DeviceTokenList>>> Handle(GetUserDeviceTokenListRequest request, CancellationToken cancellationToken)
        {
            var result=await _deviceRegistrationReadRepository.GetWhere(x=>x.InternalGUID== request.InternalGUID,false).ToListAsync();
            var mapping = _mapper.Map<List<DeviceTokenList>>(result);
            return new Response<List<DeviceTokenList>>(mapping);
        }
    }
}
