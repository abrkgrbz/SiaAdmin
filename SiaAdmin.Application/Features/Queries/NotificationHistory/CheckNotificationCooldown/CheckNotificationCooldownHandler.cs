using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Application.Repositories.NotificationHistory;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.CheckNotificationCooldown
{
    public class CheckNotificationCooldownHandler:IRequestHandler<CheckNotificationCooldownRequest, CheckNotificationCooldownResponse>
    {

        private readonly INotificationHistoryReadRepository _notificationHistoryReadRepository;
        private readonly IDeviceRegistrationsReadRepository _deviceRegistrationsReadRepository;
        private readonly IMapper _mapper;
        public CheckNotificationCooldownHandler(INotificationHistoryReadRepository notificationHistoryReadRepository, IMapper mapper, IDeviceRegistrationsReadRepository deviceRegistrationsReadRepository)
        {
            _notificationHistoryReadRepository = notificationHistoryReadRepository;
            _mapper = mapper;
            _deviceRegistrationsReadRepository = deviceRegistrationsReadRepository;
        }

        public async Task<CheckNotificationCooldownResponse> Handle(CheckNotificationCooldownRequest request, CancellationToken cancellationToken)
        {
            var result = await _notificationHistoryReadRepository.CheckNotificationCooldownAsync(request.SurveyId);  
            var mapping = _mapper.Map<CheckNotificationCooldownResponse>(result); 
            return mapping;
        }
    }
}
