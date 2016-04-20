using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PlantSitterShared.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestForTimeline
    {
        [TestMethod]
        public async Task TestUploadTimelineDataCase0()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.UploadData(1, 15, 1, 25, 30, 200, DateTime.Now.ToString("yyyy/MM/dd hh:mm::ss"), CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task TestUploadTimelineDataCase1()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.UploadData(1000, 15, 1, 25, 30, 200, DateTime.Now.ToString("yyyy/MM/dd hh:mm::ss"), CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(!result.IsSuccessful);
            Assert.IsTrue(result.ErrorCode==406);
        }

        [TestMethod]
        public async Task TestUploadTimelineDataCase2()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.UploadData(1, 15000, 1, 25, 30, 200, DateTime.Now.ToString("yyyy/MM/dd hh:mm::ss"), CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(!result.IsSuccessful);
            Assert.IsTrue(result.ErrorCode == 300);
        }

        [TestMethod]
        public async Task TestGetTimelineDataCase0()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.GetTimelineData(15, "", "", CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task TestGetTimelineDataCase1()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.GetTimelineData(15, "", "", CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
