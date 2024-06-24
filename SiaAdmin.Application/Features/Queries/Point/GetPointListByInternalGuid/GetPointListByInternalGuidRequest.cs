using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.Point.GetPointListByInternalGuid
{
    public class GetPointListByInternalGuidRequest:IRequest<GetPointListByInternalGuidResponse>
    {
        public string SurveyUserGUID { get; set; }
    }
}
