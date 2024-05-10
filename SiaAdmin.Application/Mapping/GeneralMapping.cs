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
using SiaAdmin.Application.Features.Commands.SurveyAssigned.CreateSurveyAssigned;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<CreateSurvey, CreateSurveyRequest>().ReverseMap();
            CreateMap<Survey, CreateSurvey>().ReverseMap();
            CreateMap<Survey, ListSurvey>().ReverseMap();
            CreateMap<SurveyAssigned, ListSurveyAssigned>().ReverseMap();
            CreateMap<Survey, MapSurveyAssigned>().ReverseMap();
            CreateMap<FilterData, ListFilterData>().ReverseMap();
        }
    }

}
