using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetSurveyUserList
{
    public class GetSurveyUserListViewModel
    {
        public Guid SurveyUserGUID { get; set; }
        public int SurveyId { get; set; }
        public string SurveyText { get; set; }
        public string SurveyDescription { get; set; }
        public string SurveyLink { get; set; }
        public string SurveyLinkText { get; set; }    
        public int SurveyPoints { get; set; } 
    }
}
