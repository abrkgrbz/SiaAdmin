using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Dashboard.GetNumberOfPanelist
{
    public class GetNumberOfPanelistHandler:IRequestHandler<GetNumberOfPanelistRequest,GetNumberOfPanelistResponse>
    {
        
        public async Task<GetNumberOfPanelistResponse> Handle(GetNumberOfPanelistRequest request, CancellationToken cancellationToken)
        {
             
            throw new NotImplementedException();
        }
    }
}
