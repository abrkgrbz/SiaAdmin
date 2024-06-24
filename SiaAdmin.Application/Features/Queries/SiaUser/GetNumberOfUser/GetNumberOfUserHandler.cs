using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetNumberOfUser
{
    public class GetNumberOfUserHandler:IRequestHandler<GetNumberOfUserRequest,GetNumberOfUserResponse>
    {
        private IUserReadRepository _userReadRepository;

        public GetNumberOfUserHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetNumberOfUserResponse> Handle(GetNumberOfUserRequest request, CancellationToken cancellationToken)
        {
            var isActive = await _userReadRepository.GetWhere(x=>x.Active==1,false).ToListAsync();
            var isNotActive= await _userReadRepository.GetWhere(x => x.Active == 0, false).ToListAsync();
            if (request.isDistinct)
            {
                isActive = isActive.DistinctBy(x=>x.Msisdn).ToList();
                isNotActive = isNotActive.DistinctBy(x=>x.Msisdn).ToList();
            }
            List<GetNumberOfUserViewModel> model=new List<GetNumberOfUserViewModel>();
            model.Add(new GetNumberOfUserViewModel()
            {
                Adet =isActive.Count,
                Durum = 1
            });
            model.Add(new GetNumberOfUserViewModel()
            {
                Adet =isNotActive.Count,
                Durum = 0
            });

            return new(){data = model};
        }
    }
}
