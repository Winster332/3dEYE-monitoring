using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadiusVision.RVParser;

namespace RadiusVision.BddTests
{
	[TestClass]
	public class RadiusVisionParserTests : RVTests
	{
		[TestMethod]
		public void RVParser_CheckCount_ReturnValidResult()
		{
			var parser = new RadiusVisionParser();
			var isComplated = false;

			parser.GetCamerasList(cameras => 
			{
				isComplated = true;
				Assert.AreNotEqual<int>(0, cameras.Count);
			});

			Wait(ref isComplated, 1000);
		}
	}
}
