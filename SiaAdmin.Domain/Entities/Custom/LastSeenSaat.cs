using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiaAdmin.Domain.Entities.Custom
{
	[Keyless]
	public class LastSeenSaat
	{
		public string LastSeen { get; set; }
		public Int64 ProfilYasamSaatDegeri { get; set; }
	}
}
