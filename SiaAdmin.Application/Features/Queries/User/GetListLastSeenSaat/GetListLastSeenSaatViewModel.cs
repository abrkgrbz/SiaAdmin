using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetListLastSeenSaat
{
	public class GetListLastSeenSaatViewModel
	{
        [JsonPropertyName("lastseen")]
        public string LastSeen { get; set; }
        [JsonPropertyName("adet")]
        public Int64 ProfilYasamSaatDegeri { get; set; }
    }
}
