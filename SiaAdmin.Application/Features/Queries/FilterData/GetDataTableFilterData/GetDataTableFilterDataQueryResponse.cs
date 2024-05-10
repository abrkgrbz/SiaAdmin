using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.FilterData.GetDataTableFilterData
{
    public class GetDataTableFilterDataQueryResponse
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordTotal { get; set; }
        public object FilterDatas { get; set; }
    }
}
