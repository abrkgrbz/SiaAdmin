using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.SurveyAssigned.GetUserGuidBySurveyAssigned
{
    public class GetUserGuidBySurveyAssignedResponse
    {
        public List<Guid> InternalGUIDs { get; set; }

        public GetUserGuidBySurveyAssignedResponse()
        {
            InternalGUIDs=new List<Guid>();
        }
    }
     
}
