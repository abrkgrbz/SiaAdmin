using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLog
{
    public class CreateSurveyLogRequest:IRequest<CreateSurveyLogResponse>
    {
        public IFormFile ExcelFile { get; set; }
    }
}
