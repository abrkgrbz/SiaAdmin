using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Queries.EODTable
{
    public class EODTableQueryRequest:IRequest<EODTableQueryResponse> 
    {
        public int draw { get; set; } 
        public int Length { get; set; }
        public int Start { get; set; } 
        public string? orderColumnIndex { get; set; }
        public string? orderDir { get; set; }
        public string? orderColumnName { get; set; }
        public string? searchValue { get; set; }
    }
}
