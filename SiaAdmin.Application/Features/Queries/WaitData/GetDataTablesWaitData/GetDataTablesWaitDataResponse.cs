using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.WaitData.GetDataTablesWaitData
{
    public class GetDataTablesWaitDataResponse
    {
        public int recordsFiltered { get; set; }
        public int recordTotal { get; set; }
        public object data { get; set; }
    }
}
