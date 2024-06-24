using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiaAdmin.Application.DTOs.Survey;

namespace SiaAdmin.Application.Features.Queries.Survey.GetSelectListItemSurvey
{
    public class GetSelectListItemSurveyResponse
    {
      public List<SelectListItem> SurveySelectListItems { get; set; }
    }
}
