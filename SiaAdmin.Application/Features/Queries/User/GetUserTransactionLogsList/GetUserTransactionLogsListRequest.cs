using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserTransactionLogsList
{
    public class GetUserTransactionLogsListRequest:IRequest<Response<List<UserTransactionViewModel>>>
    {
        public string UserGUID { get; set; }
    }
}
