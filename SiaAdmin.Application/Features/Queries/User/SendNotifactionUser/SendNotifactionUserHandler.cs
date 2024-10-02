using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.Firebase;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.SendNotifactionUser
{
    public class SendNotifactionUserHandler : IRequestHandler<SendNotifactionUserRequest, Response<NotificationResponse>>
    {

        private IDeviceRegistrationsReadRepository _deviceRegistrationsReadRepository;
        private ISurveyAssignedReadRepository _surveyAssignedReadRepository;
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;
        public SendNotifactionUserHandler(IDeviceRegistrationsReadRepository deviceRegistrationsReadRepository, ISurveyAssignedReadRepository surveyAssignedReadRepository, IUserReadRepository userReadRepository, IMapper mapper)
        {
            _deviceRegistrationsReadRepository = deviceRegistrationsReadRepository;
            _surveyAssignedReadRepository = surveyAssignedReadRepository;
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<NotificationResponse>> Handle(SendNotifactionUserRequest request, CancellationToken cancellationToken)
        {
            NotificationResponse response = new();
            List<UserActiveSurveys> userActiveSurveysList = new();

            var tokenList = _deviceRegistrationsReadRepository.GetAll(false)
                .GroupBy(x => new { x.InternalGUID, x.DeviceIdToken })
                .Select(x => new UserTokenList() { UsersTokensList = x.Key.DeviceIdToken, InternalGUID = x.Key.InternalGUID })
                .ToList();

            foreach (var userDevice in tokenList)
            {
                var data = _surveyAssignedReadRepository
                     .GetWhere(x => x.InternalGuid == userDevice.InternalGUID && x.SurveyActive == 1)
                     .OrderByDescending(x => x.Timestamp).FirstOrDefault();

                var userData = _userReadRepository.GetUserSurveyList(userDevice.InternalGUID)
                    .Where(x => x.SurveyActive == 1).OrderByDescending(x=>x.TimeStamp).FirstOrDefault();
                if (userData != null)
                {
                    var mappingProfile = _mapper.Map<UserActiveSurveys>(userData);
                    userActiveSurveysList.Add(new UserActiveSurveys()
                    {
                        SurveyLink = mappingProfile.SurveyLink, 
                        DeviceTokeId = userDevice.UsersTokensList
                    });
                }

            }
            response.UserSurveys = userActiveSurveysList;
            return new Response<NotificationResponse>(response);
        }

        public class UserActiveSurveys
        { 
            public string DeviceTokeId { get; set; } 
            public string SurveyLink { get; set; }
        }


        public class UserTokenList
        {
            public string UsersTokensList { get; set; }
            public Guid InternalGUID { get; set; }
        }
    }
}
