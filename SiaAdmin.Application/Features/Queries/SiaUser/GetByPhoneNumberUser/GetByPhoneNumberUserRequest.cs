﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.SiaUser.GetByPhoneNumberUser
{
    public class GetByPhoneNumberUserRequest:IRequest<GetByPhoneNumberUserResponse>
    {
        public string PhoneNumber { get; set; }
    }
}
