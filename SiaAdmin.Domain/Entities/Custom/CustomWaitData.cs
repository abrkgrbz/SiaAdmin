using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Custom
{
    [Keyless]
    public class CustomWaitData
    {
        public int SurveyId { get; set; }
        public DateTime Tarih { get; set; }
        public int Adet { get; set; }
    }
}
