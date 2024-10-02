using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.Firebase
{
    public interface IAuthService
    {
        Task<string> Login(string phoneNumber);
    }
}
