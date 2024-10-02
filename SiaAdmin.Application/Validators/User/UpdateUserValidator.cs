using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.User.UpdateUserProfile;

namespace SiaAdmin.Application.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserProfileRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ad boş bırakılamaz");
            RuleFor(x => x.Surname)
                .NotNull()
                .NotEmpty()
                .WithMessage("Soyad boş bırakılamaz");
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email boş bırakılamaz")
                .EmailAddress().WithMessage("Lütfen geçerli bir email adresi giriniz");
            RuleFor(x => x.Birthdate)
                .NotNull()
                .NotEmpty()
                .WithMessage("Doğum yılı boş bırakılamaz");
            RuleFor(x => x.Location)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ikametgah Edilen il boş bırakılamaz");
            RuleFor(x => x.Sex)
                .NotNull()
                .NotEmpty()
                .WithMessage("Cinsiyet boş bırakılamaz");
        }
    }
}
