using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.DTOs.Survey;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Survey.GetSelectListItemSurvey
{
    public class GetSelectListItemSurveyHandler:IRequestHandler<GetSelectListItemSurveyRequest,GetSelectListItemSurveyResponse>
    {
        private readonly ISurveyReadRepository _surveyReadRepository;

        public GetSelectListItemSurveyHandler(ISurveyReadRepository surveyReadRepository)
        {
            _surveyReadRepository = surveyReadRepository;
        }

        public async Task<GetSelectListItemSurveyResponse> Handle(GetSelectListItemSurveyRequest request, CancellationToken cancellationToken)
        {
            var listSurvey =await _surveyReadRepository.GetWhere(x => x.Mandatory == 0 && x.Id != 1, false)
                .OrderByDescending(x => x.Id).ToListAsync();
           List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in listSurvey)
            {
                 list.Add(new SelectListItem()
                 {
                     Value = item.Id.ToString(),
                     Text = item.Id + "-" + item.SurveyText
                 });
            }

            return new()
            {
                SurveySelectListItems = list
            };
        }
    }
}
