using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.User.CreateUser;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly ISiaUserReadRepository _siaUserReadRepository;

        public CreateUserValidator(ISiaUserReadRepository siaUserReadRepository)
        {
            _siaUserReadRepository = siaUserReadRepository;
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Ad alani, zorunludur ");
            RuleFor(x => x.Surname)
                .NotEmpty()
                .NotNull()
                .WithMessage("Soyad alani zorunludur ");
            RuleFor(x => x.Username)
                .NotEmpty()
                .NotNull()
                .WithMessage("Kullanici adı alani zorunludur ");
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Şifre alanı zorunludur ");
            RuleFor(x => x.Username).Must(username =>
            {
                bool exists = _siaUserReadRepository.IsUniqueUsername(username);
                return exists;

            }).WithMessage("Bu kullanici adi kayitli!");

        }


    }
}
