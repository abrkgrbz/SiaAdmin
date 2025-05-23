﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Repositories
{
    public interface ISiaUserReadRepository:IReadRepository<SiaUser>
    {
        public bool IsUniqueUsername(string username);
    }
}
