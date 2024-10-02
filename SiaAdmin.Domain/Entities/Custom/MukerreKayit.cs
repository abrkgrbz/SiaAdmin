using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Custom
{
    [Keyless]
    public class MukerreKayit
    {
        public Guid InternalGUID { get; set; }
        public int SurveyId { get; set; }
    }
}
