using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.Dashboard.GetPassiveUserStats
{
    public class GetPassiveUserStatsRequest:IRequest<List<GetPassiveUserStatsResponse>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
