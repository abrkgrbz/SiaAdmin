using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories.OTPHistory;

namespace SiaAdmin.Application.Features.Queries.OTPHistory.GetOTPHistory
{
    public class GetOTPHistoryHandler:IRequestHandler<GetOTPHistoryRequest,GetOTPHistoryResponse>
    {
        private readonly IOTPHistoryReadRepository _otpHistoryReadRepository;

        public GetOTPHistoryHandler(IOTPHistoryReadRepository otpHistoryReadRepository)
        {
            _otpHistoryReadRepository = otpHistoryReadRepository;
        }

        public async Task<GetOTPHistoryResponse> Handle(GetOTPHistoryRequest request, CancellationToken cancellationToken)
        {
            var lastOTPCode = await _otpHistoryReadRepository.GetWhere(x => x.Msisdn.Equals(request.PhoneNumber))
                .OrderBy(x => x.Timestamp).LastOrDefaultAsync();
            if (lastOTPCode == null)
                throw new ApiException("Bu numaraya ait OTP kodu bulunamadı");
            return new GetOTPHistoryResponse() { Code = lastOTPCode.Otp };
        }
    }
}
