using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserProfile
{
    public class GetUserProfileHandler: IRequestHandler<GetUserProfileRequest, Response<GetUserProfileViewModel>>
    {
        private IUserReadRepository _userReadRepository;
        private IMapper _mapper;
        public GetUserProfileHandler(IUserReadRepository userReadRepository, IMapper mapper)
        {
            _userReadRepository = userReadRepository;
            _mapper = mapper;
        }


        public async Task<Response<GetUserProfileViewModel>> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
        {
            
            Guid parseGuid = Guid.Parse(request.UserGuid); 
            var result = _userReadRepository.GetUserProfile(parseGuid);
            var contactSetting = getUserContactSettings(result.totalmessaging); 
            var mappingProfile = _mapper.Map<GetUserProfileViewModel>(result);
            mappingProfile.ContactSettings=contactSetting;
            return new Response<GetUserProfileViewModel>(mappingProfile);
        }

        private ContactSettings getUserContactSettings(int? totalmessaging)
        {
            switch (totalmessaging)
            {
                case 7:
                    return new ContactSettings() { IsCheckedEmail = true, IsCheckedPhone = true, IsCheckedSms = true };
                break;
                case 5:
                    return new ContactSettings() { IsCheckedEmail = false, IsCheckedPhone = true, IsCheckedSms = true };
                break;
                case 3:
                    return new ContactSettings() { IsCheckedEmail = true, IsCheckedPhone = false, IsCheckedSms = true };
                break;
                case 6:
                    return new ContactSettings() { IsCheckedEmail = true, IsCheckedPhone = true, IsCheckedSms = false };
                break;
                case 2:
                    return new ContactSettings() { IsCheckedEmail = true, IsCheckedPhone = false, IsCheckedSms = false };
                break;
                case 1:
                    return new ContactSettings() { IsCheckedEmail = false, IsCheckedPhone = false, IsCheckedSms = true };
                break;
                case 4:
                    return new ContactSettings() { IsCheckedEmail = false, IsCheckedPhone = true, IsCheckedSms = false };
                break;
                case 0:
                    return new ContactSettings() { IsCheckedEmail = false, IsCheckedPhone = false, IsCheckedSms = false };
                break;
            }

            throw new ApiException("Iletisim bilgileri okunamadı");
        }
    }
}
