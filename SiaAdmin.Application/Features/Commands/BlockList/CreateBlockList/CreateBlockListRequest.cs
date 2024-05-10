using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace SiaAdmin.Application.Features.Commands.BlockList.CreateBlockList
{
    public class CreateBlockListRequest:IRequest<CreateBlockListResponse>
    {
        private int Active { get; set; } = 1;
        private int RecType { get; set; } = 2;
        private DateTime Timestamp { get; set; }=DateTime.Now;
        public int IptalKodu { get; set; }
        public IFormFile BlockedUserExcelFile { get; set; }
        public string Note { get; set; }
    }
}
