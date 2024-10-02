using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetSurveyUserList
{
    public class GetSurveyUserListRequest:IRequest<Response<List<GetSurveyUserListViewModel>>>
    {
        public string UserGUID { get; set; }
    }
}
