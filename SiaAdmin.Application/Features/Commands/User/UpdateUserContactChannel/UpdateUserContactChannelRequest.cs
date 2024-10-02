using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserContactChannel
{
    public class UpdateUserContactChannelRequest:IRequest<Response<bool>>
    {
        public bool IsCheckedSms { get; set; }
        public bool IsCheckedPhone { get; set; }
        public bool IsCheckedEmail { get; set; }
        public Guid InternalGUID { get; set; }
    }
}
