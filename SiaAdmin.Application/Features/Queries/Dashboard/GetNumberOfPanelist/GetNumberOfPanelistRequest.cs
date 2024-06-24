using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.Dashboard.GetNumberOfPanelist
{
    public class GetNumberOfPanelistRequest:IRequest<GetNumberOfPanelistResponse>
    {
        public bool isDistinct { get; set; }
    }
}
