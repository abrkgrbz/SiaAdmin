using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories.OTPHistory;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.OTPHistory
{
    public class OTPHistoryWriteRepository:WriteRepository<OtpHistory>,IOTPHistoryWriteRepository
    {
        public OTPHistoryWriteRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
