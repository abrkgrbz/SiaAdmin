using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Commands.Employee.ApproveEmployee
{
    public class ApproveEmployeeRequest:IRequest<ApproveEmployeeResponse>
    {
        public int userId { get; set; }
        public int roleId { get; set; }
    }
}
