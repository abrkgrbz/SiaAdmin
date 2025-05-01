using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokensCount
{
    public class GetUserDeviceTokensCountResponse
    {
        [JsonPropertyName("recipientCountElement")]
        public int CountUsers { get; set; }
    }
}
