﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.EODPTable
{
    public class EODPTableQueryResponse
    {
        public int recordsFiltered { get; set; }
        public int recordTotal { get; set; }
        public object data { get; set; }
    }
}
