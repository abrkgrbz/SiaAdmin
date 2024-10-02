using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserProfile
{
    public class UpdateUserProfileRequest:IRequest<Response<int>>
    {
        
         
        public string Email { get; set; }
          
        public string Name { get; set; }

        public string Surname { get; set; }

        public int Birthdate { get; set; }

        public int Sex { get; set; }

        public int Location { get; set; }
        public Guid InternalGuid { get; set; }
         
    }
}
