using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Persistence.Contexts;

namespace SiaAdmin.Persistence.Repositories.DeviceRegistrations
{
    public class DeviceRegistrationsWriteRepository:WriteRepository<Domain.Entities.Models.DeviceRegistrations>,IDeviceRegistrationsWriteRepository
    {
        public DeviceRegistrationsWriteRepository(SiaAdminDbContext context) : base(context)
        {
        }
    }
}
