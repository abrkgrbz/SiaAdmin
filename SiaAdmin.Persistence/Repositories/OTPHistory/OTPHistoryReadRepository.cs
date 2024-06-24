using SiaAdmin.Application.Repositories.OTPHistory;
using SiaAdmin.Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.OTPHistory
{
    public class OTPHistoryReadRepository: ReadRepository<OtpHistory>,IOTPHistoryReadRepository
    {
        public OTPHistoryReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
