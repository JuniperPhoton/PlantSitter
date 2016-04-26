using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PlantSitterShared.API;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestForPlant
    {
        [TestMethod]
        public async Task TestGetPlantInfoCase1()
        {
            //Case1
            Helper.AddAuthInfo();
            var result = await CloudService.GetPlantInfo(1, CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task TestGetPlantInfoCase2()
        {
            //Case1
            Helper.AddAuthInfo();
            //Case2
            var result2 = await CloudService.GetPlantInfo(10000, CTSFactory.MakeCTS().Token);
            result2.ParseAPIResult();
            Assert.IsTrue(!result2.IsSuccessful);
            Assert.IsTrue(result2.ErrorCode == 500);
        }

        [TestMethod]
        public async Task TestGetPlantInfoCase3()
        {
            Helper.AddAuthInfo();
            //Case2
            var result2 = await CloudService.GetPlantInfo(10000, CTSFactory.MakeCTS().Token);
            result2.ParseAPIResult();
            Assert.IsTrue(!result2.IsSuccessful);
            Assert.IsTrue(result2.ErrorCode == 500);
        }

        [TestMethod]
        public async Task TestSearchCase1()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.SearchPlant(1, null, null, CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task TestSearchCase2()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.SearchPlant(null, null,"Sun", CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task TestSearchCase3()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.SearchPlant(null,"向日葵",null,CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task TestSearchCase4()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.SearchPlant(null, "向", null, CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public async Task TestSearchCase5()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.SearchPlant(null, "向日葵亏", null, CTSFactory.MakeCTS().Token);
            result.ParseAPIResult();
            Assert.IsTrue(!result.IsSuccessful);
        }
    }
}
