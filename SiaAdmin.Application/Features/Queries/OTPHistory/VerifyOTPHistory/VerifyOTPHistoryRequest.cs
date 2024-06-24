using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.OTPHistory.VerifyOTPHistory
{
    public class VerifyOTPHistoryRequest:IRequest<VerifyOTPHistoryResponse>
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
