using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.StoredProcedure.ToplamAnketBilgisi
{
    public class GetTotalSurveyInformationResponse
    {
        public List<Mapping.Profiles.Procedure.ToplamAnketBilgisi> data { get; set; }
    }
}
