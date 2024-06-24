using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetByGuidSiaUser
{
    public class GetByGuidSiaUserRequest:IRequest<GetByGuidSiaUserResponse>
    {
        public Guid SurveyUserGuid { get; set; }
    }
}
