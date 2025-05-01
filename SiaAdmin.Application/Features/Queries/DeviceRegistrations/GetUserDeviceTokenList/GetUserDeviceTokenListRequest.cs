using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokenList
{
    public class GetUserDeviceTokenListRequest:IRequest<Response<List<DeviceTokenList>>>
    {
        public int SurveyId { get; set; }
    }
}
