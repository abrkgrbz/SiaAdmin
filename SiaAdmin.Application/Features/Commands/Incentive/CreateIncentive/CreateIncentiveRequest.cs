using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SiaAdmin.Application.Features.Commands.Incentive.CreateIncentive
{
    public class CreateIncentiveRequest : IRequest<CreateIncentiveResponse>
    {
        public IFormFile files { get; set; }
    }
}
