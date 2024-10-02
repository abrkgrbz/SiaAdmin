using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Queries.OTPHistory.VerifyOTPHistory;
using SiaAdmin.Application.Repositories.OTPHistory;

namespace SiaAdmin.Application.Validators.OTPHistory
{
    public class VerifyOTPHistoryValidator : AbstractValidator<VerifyOTPHistoryRequest>
    {
        private readonly IOTPHistoryReadRepository _otPHistoryReadRepository;
        public VerifyOTPHistoryValidator(IOTPHistoryReadRepository otPHistoryReadRepository)
        {
            _otPHistoryReadRepository = otPHistoryReadRepository;
            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen OTP Kodunu giriniz");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen bir telefon numarası giriniz");

            

            RuleFor(x => x.PhoneNumber)
                .Must(IsPhoneNumber)
                .WithMessage("Lütfen geçerli bir telefon numarası giriniz");

            RuleFor(x => x.Code)
                .Must(IsCode)
                .WithMessage("Lütfen geçerli bir OTP kodu giriniz");

            When(x => IsPhoneNumber(x.PhoneNumber) && IsCode(x.Code), () =>
            {
                RuleFor(x => new { x.Code, x.PhoneNumber })
                    .Must(x => IsVerify(x.Code, x.PhoneNumber))
                    .WithMessage("Doğrulama kodunuzun süresi doldu. Yeni bir doğrulama kodu talep ediniz");
            });

        }

        private bool IsPhoneNumber(string arg)
        {
            Regex regex = new Regex(@"^[0-9]{10}$");
            return regex.IsMatch(arg);
        }
        private bool IsCode(string arg)
        {
            Regex regex = new Regex(@"^[0-9]{1,6}$");
            return regex.IsMatch(arg);
        }

        private bool IsVerify(string code, string phoneNumber)
        {
            var verifyTime = _otPHistoryReadRepository.GetWhere(x => x.Msisdn.Equals(phoneNumber) && x.Otp.Equals(code)).OrderBy(x=>x.Timestamp).LastOrDefault();

            if (TimeSpan.Compare(DateTime.Now.TimeOfDay,verifyTime.Timestamp.AddMinutes(15).TimeOfDay) !=1)
            {
                return true;

            }
            return false;
        }
    }
}
