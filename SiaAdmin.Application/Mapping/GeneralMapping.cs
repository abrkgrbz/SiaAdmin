using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SiaAdmin.Application.DTOs.FilterData;
using SiaAdmin.Application.DTOs.Survey;
using SiaAdmin.Application.DTOs.SurveyAssigned;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Features.Commands.User.CreateUser;
using SiaAdmin.Application.Features.Queries.Point.GetPointListByInternalGuid;
using SiaAdmin.Application.Features.Queries.Point.GetPointListBySurveyId;
using SiaAdmin.Application.Features.Queries.SiaUser.GetByGuidSiaUser;
using SiaAdmin.Application.Features.Queries.User;
using SiaAdmin.Application.Mapping.Profiles.Procedure;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Domain.Entities.Procedure;
using ToplamAnketBilgisi = SiaAdmin.Application.Mapping.Profiles.Procedure.ToplamAnketBilgisi;

namespace SiaAdmin.Application.Mapping
{
    public class GeneralMapping:Profile
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
            CreateMap<GetByGuidSiaUserViewModel, SurveyLog>().ReverseMap();
            CreateMap<SiaUser, UserViewModel>().ReverseMap();
            CreateMap<CreateUserRequest,SiaUser>().ReverseMap();
        }
    }

}
