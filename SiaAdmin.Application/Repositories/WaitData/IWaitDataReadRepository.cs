﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Custom;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Repositories
{
    public interface IWaitDataReadRepository:IReadRepository<WaitData>
    {
        Task<List<CustomWaitData>> GetAllWaitData();
    }
}
