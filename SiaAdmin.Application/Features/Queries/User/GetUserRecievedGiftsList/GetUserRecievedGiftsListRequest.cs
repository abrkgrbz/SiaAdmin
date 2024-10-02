using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.User.GetUserRecievedGiftsList
{
    public class GetUserRecievedGiftsListRequest:IRequest<Response<List<UserRecievedGiftViewModel>>>
    {
        public string UserGUID { get; set; }
    }
}
