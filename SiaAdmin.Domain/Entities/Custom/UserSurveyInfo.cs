using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Custom
{
    [Keyless]
    public class UserSurveyInfo
    {
        public string Tarih { get; set; }
        public string SurveyText { get; set; }
        public int SurveyPoints { get; set; }
        public int Active { get; set; }
        public int Approved { get; set; }
    }
}
