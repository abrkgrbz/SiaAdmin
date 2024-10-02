using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Domain.Entities.Custom
{
	[Keyless]
	public class LastSeenAdet
	{
		public string LastSeen { get; set; }
		public int Adet { get; set; }
	}
}
