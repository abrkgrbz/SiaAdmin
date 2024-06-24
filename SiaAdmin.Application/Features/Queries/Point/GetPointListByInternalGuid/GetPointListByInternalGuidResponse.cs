using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.Point.GetPointListByInternalGuid
{
    public class GetPointListByInternalGuidResponse
    {
        public List<GetPointListViewModel> GetPointListViewModels { get; set; }

        public int ResponseCode { get; set; }
    }
}
