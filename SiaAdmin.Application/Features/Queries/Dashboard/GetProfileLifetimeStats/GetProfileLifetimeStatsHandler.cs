using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Dashboard.GetProfileLifetimeStats
{
    public class GetProfileLifetimeStatsHandler:IRequestHandler<GetProfileLifetimeStatsRequest,List<GetProfileLifetimeStatsResponse>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetProfileLifetimeStatsHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<List<GetProfileLifetimeStatsResponse>> Handle(GetProfileLifetimeStatsRequest request, CancellationToken cancellationToken)
        {
            
            var startDate = request.StartDate ?? DateTime.Now.AddMonths(-4);
            var endDate = request.EndDate ?? DateTime.Now;

      
            var totalUserCount = await _userReadRepository
                .GetWhere(u => u.Active == 0 &&
                            u.LastLogin >= startDate &&
                            u.LastLogin <= endDate)
                .CountAsync(cancellationToken);

            var takeCount = (int)(totalUserCount * 0.9); // TOP 90 PERCENT
 
            var users = await _userReadRepository
                .GetWhere(u => u.Active == 0 &&
                            u.LastLogin >= startDate &&
                            u.LastLogin <= endDate)
                .OrderBy(u => EF.Functions.DateDiffSecond(u.RegistrationDate, u.LastLogin))
                .Take(takeCount)
                .Select(u => new
                {
                    LastSeen = string.Format("{0:MM.yyyy}", u.LastLogin),
                    sss = (double)EF.Functions.DateDiffSecond(u.RegistrationDate, u.LastLogin) / 3600 // saniyeyi saate çevir
                })
                .ToListAsync(cancellationToken);

           
            var result = users
                .GroupBy(u => u.LastSeen)
                .Select(g => new GetProfileLifetimeStatsResponse()
                {
                    LastSeen = g.Key,
                    ProfilYasamSaatDegeri = Math.Round(g.Average(u => u.sss) / 10.0) * 10
                })
                .OrderBy(g => g.LastSeen)
                .ToList();

            return result;
        }
    }
}
