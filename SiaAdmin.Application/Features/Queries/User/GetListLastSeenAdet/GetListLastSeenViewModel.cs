using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.User.GetListLastSeenAdet
{
	public class GetListLastSeenViewModel
	{
		[JsonPropertyName("lastseen")]
		public string LastSeen { get; set; }
		[JsonPropertyName("adet")]
		public int Adet { get; set; }
	}
}
