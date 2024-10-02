using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SiaAdmin.Application.Interfaces.SiaUser
{
    public interface ISiaUserService
    {
        Task<string> UploadProfilePicture(IFormFile file,Guid InternalGUID);
        string GetUserProfilePicture(Guid InternalGUID);
    }
}
