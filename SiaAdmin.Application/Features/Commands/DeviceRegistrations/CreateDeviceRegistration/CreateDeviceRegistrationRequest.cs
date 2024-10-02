using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.DeviceRegistrations.CreateDeviceRegistration
{
    public class CreateDeviceRegistrationRequest:IRequest<Response<bool>>
    {
        public Guid InternalGUID { get; set; }
        public string DeviceIdToken { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }=DateTime.Now;
    }
}
