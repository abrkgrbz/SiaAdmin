using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories.OTPHistory;

namespace SiaAdmin.Application.Features.Queries.OTPHistory.VerifyOTPHistory
{
    public class VerifyOTPHistoryHandler:IRequestHandler<VerifyOTPHistoryRequest,VerifyOTPHistoryResponse>
    {
        private IOTPHistoryReadRepository _otpHistoryReadRepository;

        public VerifyOTPHistoryHandler(IOTPHistoryReadRepository otpHistoryReadRepository)
        {
            _otpHistoryReadRepository = otpHistoryReadRepository;
        }

        public async Task<VerifyOTPHistoryResponse> Handle(VerifyOTPHistoryRequest request, CancellationToken cancellationToken)
        {
            
            var otp = await _otpHistoryReadRepository
                .GetWhere(x => x.Msisdn.Equals(request.PhoneNumber) && x.Otp.Equals(request.Code)).OrderBy(x=>x.Timestamp).LastOrDefaultAsync();
            if (otp == null)
                throw new ApiException("Telefon numarası veya OTP Kodunuz hatalı!");

            return new VerifyOTPHistoryResponse() { IsVerify = true };
        }
    }
}
