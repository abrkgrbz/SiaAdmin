using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories.DeviceRegistrations;

namespace SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokensCount
{
    public class GetUserDeviceTokensCountHandler:IRequestHandler<GetUserDeviceTokensCountRequest,GetUserDeviceTokensCountResponse>
    {
        private readonly IDeviceRegistrationsReadRepository _deviceRegistrationsReadRepository;

        public GetUserDeviceTokensCountHandler(IDeviceRegistrationsReadRepository deviceRegistrationsReadRepository)
        {
            _deviceRegistrationsReadRepository = deviceRegistrationsReadRepository;
        }

        public async Task<GetUserDeviceTokensCountResponse> Handle(GetUserDeviceTokensCountRequest request, CancellationToken cancellationToken)
        {
            int count = await _deviceRegistrationsReadRepository.GetDeviceTokenIdsCount(request.SurveyId);
            return new GetUserDeviceTokensCountResponse() { CountUsers = count };
        }
    }
}
