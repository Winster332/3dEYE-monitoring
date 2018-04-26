using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiusVision.BddTests
{
	public class RVTests
	{
		public void Wait(ref bool isComplated, int timeout)
		{
			while (!isComplated)
			{
				System.Threading.Thread.Sleep(timeout);
			}
		}
	}
}
