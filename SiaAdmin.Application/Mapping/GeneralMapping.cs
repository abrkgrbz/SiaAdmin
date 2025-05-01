using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SiaAdmin.Application.DTOs.FilterData;
using SiaAdmin.Application.DTOs.Survey;
using SiaAdmin.Application.DTOs.SurveyAssigned;
using SiaAdmin.Application.Features.Commands.NotificationHistory.CreateNotificationHistory;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Features.Commands.User.CreateUser;
using SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokenList;
using SiaAdmin.Application.Features.Queries.Incentive.GetAllIncentice;
using SiaAdmin.Application.Features.Queries.NotificationHistory.CheckNotificationCooldown;
using SiaAdmin.Application.Features.Queries.Point.GetPointListByInternalGuid;
using SiaAdmin.Application.Features.Queries.Point.GetPointListBySurveyId;
using SiaAdmin.Application.Features.Queries.SiaRole.GetSiaRoles;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByGuidSiaUser;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByPhoneNumberUser;
using SiaAdmin.Application.Features.Queries.SurveyLog.GetUserLogDetail;
using SiaAdmin.Application.Features.Queries.User;
using SiaAdmin.Application.Features.Queries.User.GetAllChurnDataList;
using SiaAdmin.Application.Features.Queries.User.GetChurnDataList;
using SiaAdmin.Application.Features.Queries.User.GetListLastSeenAdet;
using SiaAdmin.Application.Features.Queries.User.GetListLastSeenSaat;
using SiaAdmin.Application.Features.Queries.User.GetSurveyUserList;
using SiaAdmin.Application.Features.Queries.User.GetUserList;
using SiaAdmin.Application.Features.Queries.User.GetUserProfile;
using SiaAdmin.Application.Features.Queries.User.GetUserRecievedGiftsList;
using SiaAdmin.Application.Features.Queries.User.GetUserSelectIncentiveList;
using SiaAdmin.Application.Features.Queries.User.GetUserSurveyInfo;
using SiaAdmin.Application.Features.Queries.User.GetUserSurveyPoint;
using SiaAdmin.Application.Features.Queries.User.GetUserTransactionLogsList;
using SiaAdmin.Application.Mapping.Helper;
using SiaAdmin.Application.Mapping.Profiles.Procedure;
using SiaAdmin.Domain.Entities.Custom;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Domain.Entities.Procedure;
using static SiaAdmin.Application.Features.Queries.User.SendNotifactionUser.SendNotifactionUserHandler;
using ToplamAnketBilgisi = SiaAdmin.Application.Mapping.Profiles.Procedure.ToplamAnketBilgisi;

namespace SiaAdmin.Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
           
            CreateMap<Survey, CreateSurveyRequest>().ReverseMap();
            CreateMap<Survey, CreateSurvey>().ReverseMap();
            CreateMap<Survey, ListSurvey>().ReverseMap();
            CreateMap<SurveyAssigned, ListSurveyAssigned>().ReverseMap();
            CreateMap<Survey, MapSurveyAssigned>().ReverseMap();
            CreateMap<FilterData, ListFilterData>().ReverseMap();
            CreateMap<ToplamAnketBilgisi, Domain.Entities.Procedure.ToplamAnketBilgisi>().ReverseMap();
            CreateMap<TanitimAnketiDolduran, TanitimDolduran>().ReverseMap();
            CreateMap<GetPointListViewModel, SurveyLog>().ReverseMap();
            CreateMap<GetPointListBySurveyIdViewModel, SurveyLog>().ReverseMap();
            CreateMap<GetUserLogDetailViewModel, SurveyLog>().ReverseMap();
            CreateMap<SiaUser, UserViewModel>().ReverseMap();
            CreateMap<CreateUserRequest, SiaUser>().ReverseMap();
            CreateMap<GetUserProfileViewModel, UserProfile>().ReverseMap();
            CreateMap<IncentiveViewModel, Incentive>().ReverseMap()
                .ForMember(dest=>dest.ImagePath,opt=>opt.MapFrom(src=>RevizeImagePath(src.ImagePath)))
                .ForMember(dest => dest.Description, opt =>
                    opt.ConvertUsing(new HtmlToTextConverter(), src => src.Description)); 
            CreateMap<UserSurveyInfo, GetUserSurveyInfoViewModel>().ReverseMap();
            CreateMap<UserTransactionLogs, UserTransactionViewModel>().ReverseMap();
            CreateMap<UserSurveyPoints, UserSurveyPointViewModel>().ReverseMap();
            CreateMap<UserSelectIncentive, UserSelectIncentiveViewModel>().ReverseMap();
            CreateMap<UserRecievedGifts, UserRecievedGiftViewModel>().ReverseMap();
            CreateMap<SiaRolesViewModel, SiaRole>().ReverseMap();
            CreateMap<DeviceRegistrations, DeviceTokenList>().ReverseMap();
            CreateMap<GetUserByGuidViewModel, User>().ReverseMap(); 
            CreateMap<GetUserLogDetailViewModel, SurveyLog>().ReverseMap();
            CreateMap<ChurnData, ChurnDataViewModel>().ReverseMap();
            CreateMap<SiaUser, UserListViewModel>().ForMember(x => x.Fullname, opt => opt.MapFrom(src => src.Name + " " + src.Surname)).ReverseMap();

            CreateMap<User, GetUserPhoneNumberViewModel>()
                .ForMember(x => x.Guid, opt
                    => opt.MapFrom(src => src.SurveyUserGuid.ToString()))
                .ForMember(x => x.PhoneNumber, opt
                    => opt.MapFrom(src => src.RegionCode + src.Msisdn))
                .ForMember(x => x.UserGUID, opt
                    => opt.MapFrom(src => src.InternalGuid))
                .ReverseMap();

            CreateMap<UserSurvey, GetSurveyUserListViewModel>()
                .ForMember(x => x.SurveyLink,
                    opt => opt.MapFrom<CustomResolver, string>(src => src.SurveyLink))
                .ReverseMap();

            CreateMap<LastSeenAdet, GetListLastSeenViewModel>().ReverseMap(); 
            CreateMap<LastSeenSaat, GetListLastSeenSaatViewModel>().ReverseMap(); 
            CreateMap<UserSurvey, UserActiveSurveys>().ForMember(x => x.SurveyLink,
                    opt => opt.MapFrom<CustomResolver, string>(src => src.SurveyLink))
                .ReverseMap();

            CreateMap<CheckNotificationCooldownResponse, NotificationCooldownResult>().ReverseMap();

            CreateMap<NotificationHistoryRequest, NotificationHistory>()
                .ForMember(dest => dest.ScheduledFor, opt => opt.MapFrom<ScheduledForValueResolver>());

            ;
        }

        private string RevizeImagePath(string imagePath)
        {

            if (!imagePath.IsNullOrEmpty())
            {
                return "https://sialive.siapanel.com/" + imagePath.Trim(); 
            }
            return imagePath;
        }
        public class HtmlToTextConverter : IValueConverter<string, string>
        {
            public string Convert(string source, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source))
                    return string.Empty;

                // HTML etiketlerini kaldır
                string result = Regex.Replace(source, "<[^>]*>", "");

                // Fazla boşlukları temizle ve trim yap
                result = Regex.Replace(result, @"\s+", " ").Trim();

                return result;
            }
        }
        public class CustomResolver : IMemberValueResolver<UserSurvey, object, string, string>
        {
            public string Resolve(UserSurvey source, object destination, string sourceMember, string destMember,
                ResolutionContext context)
            {
                var url = new Uri(sourceMember);
                string param1 = HttpUtility.ParseQueryString(url.Query).Get("g");
                string param2 = HttpUtility.ParseQueryString(url.Query).Get("s");
                string param3 = HttpUtility.ParseQueryString(url.Query).Get("c");
                param1 = source.SurveyUserGUID.ToString();
                param2 = source.SurveyId.ToString();
                param3 = LinkHelper.DoTheChecksum(param1 + "|" + param2);
                var queryParams = new Dictionary<string, string>
                {
                    {"g", param1 },
                    {"s", param2 },
                    {"c", param3 }
                };

                return LinkHelper.ModifyQueryStringManually(url.ToString(), queryParams);
            }
        }

        public class ScheduledForValueResolver : IValueResolver<NotificationHistoryRequest, NotificationHistory, DateTime>
        {
            public DateTime Resolve(NotificationHistoryRequest source, NotificationHistory destination, DateTime destMember, ResolutionContext context)
            {
               
                return source.ScheduledFor == default ? DateTime.UtcNow.AddMinutes(5) : source.ScheduledFor;
            }
        }
    }

}
