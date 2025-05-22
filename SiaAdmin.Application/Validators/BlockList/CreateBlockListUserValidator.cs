using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.User.CreateBlockUser;

namespace SiaAdmin.Application.Validators.BlockList
{
    public class CreateBlockListUserValidator:AbstractValidator<CreateBlockUserRequest>
    {
        public CreateBlockListUserValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Note)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen Not kısmını boş bırakmayınız")
                .Must(x => x.Length > 4 && x.Length < 101)
                .WithMessage("Notunuz minimum 5 maksimum 100 karakterden oluşmalıdır");

            
        }
    }
}
