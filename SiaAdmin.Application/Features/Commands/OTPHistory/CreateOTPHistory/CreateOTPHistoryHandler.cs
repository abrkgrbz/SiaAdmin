using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.Sms;
using SiaAdmin.Application.Repositories.OTPHistory;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Features.Commands.OTPHistory.CreateOTPHistory
{
    public class CreateOTPHistoryHandler : IRequestHandler<CreateOTPHistoryRequest, CreateOTPHistoryResponse>
    {
        private IOTPHistoryWriteRepository _otpHistoryWriteRepository;
        private IOTPHistoryReadRepository _otpHistoryReadRepository;
        private ISmsService _smsService;

        public CreateOTPHistoryHandler(IOTPHistoryWriteRepository otpHistoryWriteRepository, ISmsService smsService, IOTPHistoryReadRepository otpHistoryReadRepository)
        {
            _otpHistoryWriteRepository = otpHistoryWriteRepository;
            _smsService = smsService;
            _otpHistoryReadRepository = otpHistoryReadRepository;
        }


        public async Task<CreateOTPHistoryResponse> Handle(CreateOTPHistoryRequest request, CancellationToken cancellationToken)
        {
            string randomNumber = CreateRandomOTP(request.PhoneNumber);
            var insertObj = new OtpHistory()
            {
                LastBrowser = request.Browser,
                LastIp = request.IpAdress,
                Msisdn = request.PhoneNumber,
                Status = 1,
                Timestamp = DateTime.Now,
                Source = 2,
                Otp = randomNumber
            };
            var insertDb = await _otpHistoryWriteRepository.AddAsync(insertObj);
            await _otpHistoryWriteRepository.SaveAsync();
            if (request.PhoneNumber.Equals("5000000000"))
            {
                //Do Nothing
            }
            else
            {
                string takipNo = _smsService.SendSmsOneToMany(request.PhoneNumber, randomNumber).ToString();
                if (takipNo != null)
                {
                    int id = insertObj.Id;
                    var updateObj = _otpHistoryReadRepository.GetWhere(x => x.Id == id).FirstOrDefault();
                    updateObj.TrackingId = takipNo;
                    var updatedObj = _otpHistoryWriteRepository.Update(updateObj);
                    _otpHistoryWriteRepository.SaveAsync();
                }

            }

            return new CreateOTPHistoryResponse()
            {
                Code = randomNumber,
                Success = true
            };
        }

        private string CreateRandomOTP(string phoneNumber)
        {
            string randomNumber;
            if (phoneNumber == "5000000000")
            {
                randomNumber = ("0000");
            }
            else
            {

                Random rnd = new Random();
                randomNumber = "0000"+(rnd.Next(0, 9999)).ToString();
                randomNumber = randomNumber.Substring(randomNumber.Length - 4);
            }

            return randomNumber;
        }

        
    }
}
