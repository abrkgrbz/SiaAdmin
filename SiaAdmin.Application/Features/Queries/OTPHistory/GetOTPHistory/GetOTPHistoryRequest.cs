using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.OTPHistory.GetOTPHistory
{
    public class GetOTPHistoryRequest:IRequest<GetOTPHistoryResponse>
    {
        public string PhoneNumber { get; set; }
    }
}
