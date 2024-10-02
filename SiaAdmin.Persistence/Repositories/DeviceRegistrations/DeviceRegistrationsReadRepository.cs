using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.DeviceRegistrations
{
    public class DeviceRegistrationsReadRepository:ReadRepository<Domain.Entities.Models.DeviceRegistrations>,IDeviceRegistrationsReadRepository
    {
        public DeviceRegistrationsReadRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
