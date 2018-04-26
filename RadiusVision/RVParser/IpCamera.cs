using System;
using System.Collections.Generic;

namespace RadiusVision.RVParser
{
	public class IpCamera
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Host { get; set; }
		public IList<string> Locations { get; set; }
	}
}
