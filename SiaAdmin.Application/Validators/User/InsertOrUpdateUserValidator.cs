using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.User.InsertOrUpdateUser;

namespace SiaAdmin.Application.Validators.User
{
    public class InsertOrUpdateUserValidator:AbstractValidator<InsertOrUpdateUserRequest>
    {
        public InsertOrUpdateUserValidator()
        {
            RuleFor(x => x.RegionCode + x.Phone).NotEmpty().WithMessage("Telefon numarası bilgisi boş olamaz")
                .Matches(@"^\+(?:[0-9] ?){6,14}[0-9]$").WithMessage("Geçersiz telefon numarası");


        }
    }
}
