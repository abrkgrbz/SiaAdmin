using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Queries.OTPHistory.VerifyOTPHistory;

namespace SiaAdmin.Application.Validators.OTPHistory
{
    public class VerifyOTPHistoryValidator:AbstractValidator<VerifyOTPHistoryRequest>
    {
        public VerifyOTPHistoryValidator()
        {
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
    }
}
