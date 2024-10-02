using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.Incentive.GetAllIncentice
{
    public class GetAllIncenticeRequest:IRequest<Response<List<IncentiveViewModel>>>
    {
    }
}
