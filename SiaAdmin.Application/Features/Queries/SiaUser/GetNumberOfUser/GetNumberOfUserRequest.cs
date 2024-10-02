using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetNumberOfUser
{
    public class GetNumberOfUserRequest:IRequest<GetNumberOfUserResponse>
    {
      
        public bool isDistinct { get; set; }
    }
}
