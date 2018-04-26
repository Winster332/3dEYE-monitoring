using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadiusVision.Mjpeg;

namespace RadiusVision.BddTests
{
	[TestClass]
	public class RadiusVisionMjpegTests : RVTests
	{
		[TestMethod]
		public void RVReader_CheckBuffer_ReturnNotNullBuffer()
		{
			var reader = new MJPEGReader();
			var isComplated = false;

			reader.FrameReady += (o, e) =>
			{
				isComplated = true;
				Assert.AreNotEqual<int>(0, e.FrameBuffer.Length);
			};

			reader.ParseStream(new Uri($"http://axisview.axiscam.net:8010/mjpg/video.mjpg"), "axisdemo", "19@Axis84");

			Wait(ref isComplated, 1000);
		}
	}
}
