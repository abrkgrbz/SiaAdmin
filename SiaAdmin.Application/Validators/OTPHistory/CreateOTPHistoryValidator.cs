using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.OTPHistory.CreateOTPHistory;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.OTPHistory;

namespace SiaAdmin.Application.Validators.OTPHistory
{
    public class CreateOTPHistoryValidator:AbstractValidator<CreateOTPHistoryRequest>
    {
        private IBlockListReadRepository _blockListReadRepository;
        private IOTPHistoryReadRepository _OTPHistoryReadRepository;
        public CreateOTPHistoryValidator(IBlockListReadRepository blockListReadRepository, IOTPHistoryReadRepository otpHistoryReadRepository)
        {
            _blockListReadRepository = blockListReadRepository;
            _OTPHistoryReadRepository = otpHistoryReadRepository;

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen bir telefon numarası giriniz");

            RuleFor(x => x.Browser)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen browser bilgisi giriniz");

            RuleFor(x => x.IpAdress)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen bir IP adres bilgisi giriniz");

            RuleFor(x => x.PhoneNumber)
                .Must(IsPhoneNumber)
                .WithMessage("Lütfen geçerli bir telefon numarası giriniz");


            RuleFor(x => x.IpAdress)
                .Must(IsBlockedByIP)
                .WithMessage("Bu adresten sisteme giriş yapılamadı. Lütfen daha sonra tekrar deneyiniz.(4)");


            RuleFor(x => x.PhoneNumber)
                .Must(IsBlockedByPhoneNumber)
                .WithMessage("Bu numara ile sisteme giriş yapılamadı.(3)");


            RuleFor(x => x.IpAdress)
                .Must(CheckByIpEligibility)
                .WithMessage("Bir gün içinde yapılabilecek işlem sayısı aşıldı. Lütfen daha sonra tekrar deneyiniz.(2)");

            RuleFor(x => x.PhoneNumber)
                .Must(CheckByPhoneNumberEligibility)
                .WithMessage("Bir gün içinde yapılabilecek işlem sayısı aşıldı. Lütfen daha sonra tekrar deneyiniz.");
        }

        

        private bool IsPhoneNumber(string arg)
        {
            Regex regex = new Regex(@"^[0-9]{10}$");
            return regex.IsMatch(arg);
        }

        private bool IsBlockedByIP(string ip)
        {
            var checkBlockedUserByIP = _blockListReadRepository.GetWhere(x => x.RecType == 2 &&x.Data == ip).ToList().Count;
            if (checkBlockedUserByIP>0)
            {
                return false;
            }
            return true;
        }

        private bool IsBlockedByPhoneNumber(string phoneNumber)
        {
            var checkBlockedUserByIP = _blockListReadRepository.GetWhere(x => x.RecType == 1 && x.Data == phoneNumber ).ToList().Count;
            if (checkBlockedUserByIP > 0)
            {
                return false;
            }
            return true;
        }

        private bool CheckByIpEligibility(string ip)
        {
            var checkIP = _OTPHistoryReadRepository.GetWhere(x => x.LastIp == ip && x.Timestamp.Date>=DateTime.Now.Date).ToList().Count;
            if (checkIP < 15)
            {
                return true;
            }

            return false;
        }

        private bool CheckByPhoneNumberEligibility(string phoneNumber)
        {
            var checkIP = _OTPHistoryReadRepository.GetWhere(x => x.Msisdn == phoneNumber && x.Timestamp.Date >= DateTime.Now.Date).ToList().Count;
            if (checkIP < 5)
            {
                return true;
            }

            return false;
        }
    }
}
