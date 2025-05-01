using System;
using System.Collections.Concurrent;
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
             
            var results = new List<DeviceTokenList>(); 
            var uniqueTokens = new HashSet<string>(); 
            var tokenList = _deviceRegistrationReadRepository.GetDeviceIdTokensBySurveyId(request.SurveyId);
            var tokenListWithNotInSurvey =
                _deviceRegistrationReadRepository.GetDeviceTokensNotInSurvey(request.SurveyId);
             
            if (tokenList is not null)
            {
                foreach (var item in tokenList)
                {
                    if (uniqueTokens.Add(item))  
                    {
                        results.Add(new DeviceTokenList() { DeviceIdToken = item });
                    }
                }
            }
             
            if (tokenListWithNotInSurvey is not null)
            {
                foreach (var item in tokenListWithNotInSurvey)
                {
                    if (uniqueTokens.Add(item))  
                    {
                        results.Add(new DeviceTokenList() { DeviceIdToken = item });
                    }
                }
            }
            return new Response<List<DeviceTokenList>>(results);

        }
    }
}
