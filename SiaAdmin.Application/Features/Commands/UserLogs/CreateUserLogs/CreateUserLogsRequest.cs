using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.UserLogs.CreateUserLogs
{
    public class CreateUserLogsRequest:IRequest<Response<int>>
    {
        public string Username { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Data { get; set; }
        public string Url { get; set; }
    }
}
