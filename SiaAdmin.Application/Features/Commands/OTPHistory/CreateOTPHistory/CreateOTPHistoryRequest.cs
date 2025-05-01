using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Commands.OTPHistory.CreateOTPHistory
{
    public class CreateOTPHistoryRequest:IRequest<CreateOTPHistoryResponse>
    {
        public string RegionCode { get; set; }
        public string IpAdress { get; set; }
        public string Browser { get; set; }
        public string PhoneNumber { get; set; }

    }
}
