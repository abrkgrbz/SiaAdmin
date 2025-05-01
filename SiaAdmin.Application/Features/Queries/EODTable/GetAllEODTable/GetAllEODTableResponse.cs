using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.EODTable.GetAllEODTable
{
    public class GetAllEODTableResponse
    {
        public List<EODTableViewModel>? EodTableViewModels { get; set; }
    }
}
