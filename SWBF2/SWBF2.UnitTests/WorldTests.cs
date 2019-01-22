using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SWBF2.UnitTests
{
    [TestClass]
    [DeploymentItem("Data")]
    public class WorldTests
    {
        [TestMethod]
        public void LoadLDXFileTest()
        {
            string path = @"assets\worlds\TAN\world1\tan1.wld";
            World world = World.LoadFromFile(path);
        }
    }
}