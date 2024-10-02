using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetUserSelectIncentiveList
{
    public class UserSelectIncentiveViewModel
    {
        public int Id { get; set; }
        public string IncentiveText { get; set; }
        public int Points { get; set; }
    }
}
