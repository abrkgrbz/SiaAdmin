using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SiaAdmin.Application.Features.Commands.BlockList.CreateBlockList;

namespace SiaAdmin.Application.Validators.BlockList
{
    public class CreateBlockListValidator:AbstractValidator<CreateBlockListRequest>
    {
        public CreateBlockListValidator()
        {
            RuleFor(x => x.Note)
                .NotEmpty()
                .NotNull()
                .WithMessage("Lütfen Not kısmını boş bırakmayınız")
                .Must(x=>x.Length>4 && x.Length<101)
                .WithMessage("Notunuz minimum 5 maksimum 100 karakterden oluşmalıdır");

            RuleFor(x => x.IptalKodu)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lütfen bir Iptal kodu seçiniz")
                .Must(x => x > 0 && x < 3);

        }
    }
}
