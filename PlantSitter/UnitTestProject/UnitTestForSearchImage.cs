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
    public class UnitTestForSearchImage
    {
        [TestMethod]
        public async Task TestSearchImage()
        {
            var result = await CloudService.SearchImage("Sunflower");
            result.ParseAPIResult();
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
