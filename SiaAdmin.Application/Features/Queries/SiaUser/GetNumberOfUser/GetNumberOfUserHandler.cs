using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetNumberOfUser
{
    public class GetNumberOfUserHandler:IRequestHandler<GetNumberOfUserRequest,GetNumberOfUserResponse>
    {
        private IUserReadRepository _userReadRepository;
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private static DateTime _cacheExpiration = DateTime.UtcNow.AddHours(3).Date.AddDays(1).AddHours(21); // Türkiye'nin zaman dilimi GMT+3
        public GetNumberOfUserHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetNumberOfUserResponse> Handle(GetNumberOfUserRequest request, CancellationToken cancellationToken)
        {
            //var isActive = await _userReadRepository.GetWhere(x=>x.Active==1,false).ToListAsync();
            //var isNotActive= await _userReadRepository.GetWhere(x => x.Active == 0, false).ToListAsync();
            //if (request.isDistinct)
            //{
            //    isActive = isActive.DistinctBy(x=>x.Msisdn).ToList();
            //    isNotActive = isNotActive.DistinctBy(x=>x.Msisdn).ToList();
            //}
            //List<GetNumberOfUserViewModel> model=new List<GetNumberOfUserViewModel>();
            //model.Add(new GetNumberOfUserViewModel()
            //{
            //    Adet =isActive.Count,
            //    Durum = 1
            //});
            //model.Add(new GetNumberOfUserViewModel()
            //{
            //    Adet =isNotActive.Count,
            //    Durum = 0
            //});

            //return new(){data = model};
            
                var currentTime = DateTime.UtcNow.AddHours(3); // Türkiye'nin zaman dilimi GMT+3

                if (currentTime > _cacheExpiration)
                {
                    // Olası boş bir yanıt döndür
                    return new GetNumberOfUserResponse { data = new List<GetNumberOfUserViewModel>() };
                }

                string cacheKey = "GetNumberOfUser";
                if (!_cache.TryGetValue(cacheKey, out GetNumberOfUserResponse cachedResponse))
                {
                    var isActive = await _userReadRepository.GetWhere(x => x.Active == 1, false).ToListAsync();
                    var isNotActive = await _userReadRepository.GetWhere(x => x.Active == 0, false).ToListAsync();

                    if (request.isDistinct)
                    {
                        isActive = isActive.DistinctBy(x => x.Msisdn).ToList();
                        isNotActive = isNotActive.DistinctBy(x => x.Msisdn).ToList();
                    }

                    List<GetNumberOfUserViewModel> model = new List<GetNumberOfUserViewModel>();
                    model.Add(new GetNumberOfUserViewModel()
                    {
                        Adet = isActive.Count,
                        Durum = 1
                    });
                    model.Add(new GetNumberOfUserViewModel()
                    {
                        Adet = isNotActive.Count,
                        Durum = 0
                    });

                    cachedResponse = new GetNumberOfUserResponse { data = model };

                    // Cache'e ekle
                    _cache.Set(cacheKey, cachedResponse, _cacheExpiration - currentTime);
                }

                return cachedResponse;
        }
    }
}
