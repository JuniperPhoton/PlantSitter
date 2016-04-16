using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using PlantSitterShared.API;
using JP.Utils.Data;
using Windows.Data.Json;
using JP.Utils.Data.Json;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestForPlan
    {
        [TestMethod]
        public async Task TestGetAllPlans()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.GetAllPlans(CTSFactory.MakeCTS().Token);
            Assert.IsTrue(result.IsSuccessful);
            var json = result.JsonSrc;
            var obj = JsonObject.Parse(json);
            var plants = JsonParser.GetJsonArrayFromJsonObj(obj, "Plans");
            Assert.IsNotNull(plants);
            Assert.IsTrue(plants.Count > 0);
            foreach (var plant in plants)
            {
                var plantObj = plant.GetObject();
                Assert.IsNotNull(plantObj);
            }
        }
    }
}
