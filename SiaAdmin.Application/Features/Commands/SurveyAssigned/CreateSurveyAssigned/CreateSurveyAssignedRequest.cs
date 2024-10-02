using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Commands.SurveyAssigned.CreateSurveyAssigned
{
    public class CreateSurveyAssignedRequest:IRequest<CreateSurveyAssignedResponse>
    {
        public int SurveyId { get; set; }  
        public DateTime? SurveyValidity { get; set; }
        public DateTime? SurveyStartDate { get; set; }
        public int SurveyPoints { get; set; }
        public IFormFile ExcelFile { get; set; }

        public string userId { get; set; }
    }
}
