using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Dashboard.GetPassiveUserStats
{
    public class GetPassiveUserStatsHandler:IRequestHandler<GetPassiveUserStatsRequest,List<GetPassiveUserStatsResponse>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetPassiveUserStatsHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<List<GetPassiveUserStatsResponse>> Handle(GetPassiveUserStatsRequest request, CancellationToken cancellationToken)
        { 
            var startDate = request.StartDate ?? DateTime.Now.AddMonths(-4);
            var endDate = request.EndDate ?? DateTime.Now;
             
            var users = await _userReadRepository
                .GetWhere(u => u.Active == 0 &&
                            u.LastLogin >= startDate &&
                            u.LastLogin <= endDate)
                .Select(u => new { u.LastLogin })
                .ToListAsync(cancellationToken);

            
            var passiveUserStats = users
                .GroupBy(u => u.LastLogin.Value.ToString("MM.yyyy"))
                .Select(g => new GetPassiveUserStatsResponse()
                {
                    LastSeen = g.Key,
                    Adet = g.Count()
                })
                .OrderBy(g => g.LastSeen)
                .ToList();

            return passiveUserStats;
        }
    }
}
