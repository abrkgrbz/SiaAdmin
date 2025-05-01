using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Queries.OTPHistory.GetOTPHistory;

namespace SiaAdmin.Application.Validators.OTPHistory
{
    public class GetOTPHistoryValidator : AbstractValidator<GetOTPHistoryRequest>
    {
        public GetOTPHistoryValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Lütfen bir telefon numarası giriniz")
                .Must(IsPhoneNumber)
                .WithMessage("Lütfen geçerli bir telefon numarası giriniz");
        }

        private bool IsPhoneNumber(string arg)
        {
            Regex regex = new Regex(@"^[0-9]{10}$");
            return regex.IsMatch(arg);
        }
    }
}
