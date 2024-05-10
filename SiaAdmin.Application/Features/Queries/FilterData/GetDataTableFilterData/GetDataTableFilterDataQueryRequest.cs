using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.FilterData.GetDataTableFilterData
{
    public class GetDataTableFilterDataQueryRequest:IRequest<GetDataTableFilterDataQueryResponse>
    {
        public string draw { get; set; }
        public int length { get; set; }
        public int start { get; set; }
        public string? orderColumnIndex { get; set; }
        public string? orderDir { get; set; }
        public string? orderColumnName { get; set; }
        public string? searchValue { get; set; }
    }
}
