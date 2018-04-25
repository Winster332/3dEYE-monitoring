using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void TestRVParser()
        {
			var vision = new WpfApp3.MJPEGReader();
			vision.FrameReady += (o, e) => 
			{
				Assert.NotEmpty(e.FrameBuffer);
			};
			vision.ParseStream(new Uri($"http://axisview.axiscam.net:8010/mjpg/video.mjpg"), "axisdemo", "19@Axis84");
        }

		[Fact]
		public void TestMJPEGReader()
		{
			var parser = new WpfApp3.RadiusVisionParser();
			parser.CamerasComplated += (o, e) => 
			{
				Assert.NotEmpty(e);
			};
		}
	}
}
