using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace StreamR.Tests
{
	[TestClass]
	public class StreamSetupTests
	{
		[TestMethod]
		public void StreamListTest()
		{
			StreamManager streamManager = new StreamManager();
			_ = streamManager.RunStreamAsync("test", null);

			// 
			Assert.AreEqual(0, streamManager.ListStreams().Count);
		}
	}
}

