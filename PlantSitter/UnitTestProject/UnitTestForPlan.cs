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
            //Case 1
            Helper.AddAuthInfo();
            var result = await CloudService.GetAllPlans(CTSFactory.MakeCTS().Token);
            Assert.IsTrue(result.IsSuccessful);
            var json = result.JsonSrc;
            var obj = JsonObject.Parse(json);
            var plans = JsonParser.GetJsonArrayFromJsonObj(obj, "Plans");
            Assert.IsNotNull(plans);
            Assert.IsTrue(plans.Count > 0);
            foreach (var plant in plans)
            {
                var plantObj = plant.GetObject();
                Assert.IsNotNull(plantObj);
            }
        }

        [TestMethod]
        public async Task TestGetPlanCase1()
        {
            //Case1
            Helper.AddAuthInfo();
            var result = await CloudService.GetPlan(15,CTSFactory.MakeCTS().Token);
            Assert.IsTrue(result.IsSuccessful);
            var json = result.JsonSrc;
            var obj = JsonObject.Parse(json);
            var planObj = JsonParser.GetJsonObjFromJsonObj(obj, "Plan");
            Assert.IsNotNull(planObj);
        }

        [TestMethod]
        public async Task TestGetPlanCase2()
        {
            Helper.AddAuthInfo();
            //Case2
            var result2 = await CloudService.GetPlan(1000000, CTSFactory.MakeCTS().Token);
            result2.ParseAPIResult();
            Assert.IsFalse(result2.IsSuccessful);
            Assert.IsTrue(result2.ErrorCode == 300);
        }

        [TestMethod]
        public async Task TestSetMainPlanCase1()
        {
            Helper.AddAuthInfo();
            //Case1
            var result1 = await CloudService.SetMainPlan(15, CTSFactory.MakeCTS().Token);
            result1.ParseAPIResult();
            Assert.IsTrue(result1.IsSuccessful);
        }

        [TestMethod]
        public async Task TestGetMainPlanCase1()
        {
            Helper.AddAuthInfo();
            //Case1
            var result1 = await CloudService.GetMainPlan(CTSFactory.MakeCTS().Token);
            result1.ParseAPIResult();
            Assert.IsTrue(result1.IsSuccessful);
        }

        [TestMethod]
        public async Task TestAddPlanCase1()
        {
            Helper.AddAuthInfo();
            var result = await CloudService.AddPlan(15, "PlanTest", DateTime.Now.ToString("yyyy/MM/dd hh:mm::ss"), CTSFactory.MakeCTS().Token);
            Assert.IsTrue(result.IsSuccessful);
            var json = result.JsonSrc;
            var obj = JsonObject.Parse(json);
            var planObj = JsonParser.GetJsonObjFromJsonObj(obj, "Plan");
            Assert.IsNotNull(planObj);
        }
    }
}
