using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserReferenceCode
{
    public class UpdateUserReferenceCodeRequest:IRequest<Response<bool>>
    {
        public Guid InternalGUID { get; set; }
        public string ReferenceCode { get; set; }
    }
}
