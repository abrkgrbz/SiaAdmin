using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserContactChannel
{
    public class UpdateUserContactChannelHandler : IRequestHandler<UpdateUserContactChannelRequest, Response<bool>>
    {
        private IUserWriteRepository _userWriteRepository;
        private IUserReadRepository _userReadRepository;

        public UpdateUserContactChannelHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<Response<bool>> Handle(UpdateUserContactChannelRequest request, CancellationToken cancellationToken)
        {
            var user = _userReadRepository.GetWhere(x => x.InternalGuid == request.InternalGUID).FirstOrDefault();
            int value = ReturnCheckedValue(request.IsCheckedSms, request.IsCheckedEmail, request.IsCheckedPhone);
            user.ContactChannel = value;
            _userWriteRepository.Update(user);
            await _userWriteRepository.SaveAsync();
            return new Response<bool>(true, "İletisim tercihleriniz güncellendi");
        }

        int ReturnCheckedValue(bool sms, bool email, bool phone)
        {
            int total = 7;
            if (!phone)
            {
                total += -4;
            }

            if (!email)
            {
                total += -2;
            }

            if (!sms)
            {
                total += -1;
            }

            return total;
        }
    }
}
