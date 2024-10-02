using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserSelectIncentiveList
{
    public class GetUserSelectIncentiveListRequest:IRequest<Response<List<UserSelectIncentiveViewModel>>>
    {
        public string UserGUID { get; set; }
    }
}
