using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PlantSitterShared.API;
using System;
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
            for(int i=0;i<24;i++)
            {
                var random = new Random((int)DateTime.Now.Ticks);
                var result = await CloudService.UploadData(10, 24, random.Next(0,1), random.Next(0,50), random.Next(0,100), random.Next(1,30000), DateTime.Now.AddDays(-1).AddHours(+i).ToString("yyyy/MM/dd HH:mm"), CTSFactory.MakeCTS().Token);
                result.ParseAPIResult();
                Assert.IsTrue(result.IsSuccessful);
            }
        }

        [TestMethod]
        public async Task TestUploadTimelineDataCase1()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.UploadData(1000, 15, 1, 25, 30, 200, DateTime.Now.ToString("yyyy/MM/dd hh:mm::ss"), CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(!result.IsSuccessful);
            Assert.IsTrue(result.ErrorCode==500);
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
