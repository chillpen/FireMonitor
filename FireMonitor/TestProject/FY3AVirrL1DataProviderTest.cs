using FireMonitor.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using MapHandle;
using SharpMap.Forms;
using System.Windows.Forms;
using SharpMap.Layers;
using SharpMap;
namespace TestProject
{


    /// <summary>
    ///这是 FY3AVirrL1DataProviderTest 的测试类，旨在
    ///包含所有 FY3AVirrL1DataProviderTest 单元测试
    ///</summary>
    [TestClass()]
    public class FY3AVirrL1DataProviderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private string TestDataDir = "c:\\FireMonTestData";
        /// <summary>
        ///FY3AVirrL1DataProvider 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void FY3AVirrL1DataProviderConstructorTest()
        {
            FY3AVirrL1DataProvider target = new FY3AVirrL1DataProvider();
            // Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///GetData 的测试
        ///</summary>
        [TestMethod()]
        public void GetDataTest()
        {
            FY3AVirrL1DataProvider target = new FY3AVirrL1DataProvider(); // TODO: 初始化为适当的值
            target.L1File = TestDataDir + "\\FY3A_VIRRX_GBAL_L1_20090427_0255_1000M_MS.HDF";
            Color expected = Color.FromArgb(255, 47, 47, 47); // TODO: 初始化为适当的值

            Bitmap actual;
            actual = target.GetData();

            Color color = actual.GetPixel(500, 500);

            Assert.AreEqual(expected, color);

        }

        /// <summary>
        ///File 的测试
        ///</summary>
        [TestMethod()]
        public void FileTest()
        {
            FY3AVirrL1DataProvider target = new FY3AVirrL1DataProvider(); // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            target.L1File = expected;
            Assert.Inconclusive("无法验证只写属性。");
        }

        /// <summary>
        ///ReadLonlat 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FireMonitor.exe")]
        public void ReadLonTest()
        {


            FY3AVirrL1DataProvider provider = new FY3AVirrL1DataProvider();
            PrivateObject param0 = new PrivateObject(provider); // TODO: 初始化为适当的值
            FY3AVirrL1DataProvider_Accessor target = new FY3AVirrL1DataProvider_Accessor(param0); // TODO: 初始化为适当的值
            target.L1File =TestDataDir+"\\FY3A_VIRRX_GBAL_L1_20090427_0255_1000M_MS.HDF";

            float[, ,] lon = target.ReadLon();

            bool ret = false;

            if (lon.Length > 0 && lon[0, 0, 1] != 0)
                ret = true;

            Assert.AreEqual(ret, true);
            //Assert.Inconclusive("无法验证不返回值的方法。");
        }


        /// <summary>
        /// 的测试
        ///</summary>
        // [TestMethod()]
        //public void 

        /// <summary>
        ///CreateBorder 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FireMonitor.exe")]
        public void CreateBorderTest()
        {
            FY3AVirrL1DataProvider_Accessor provider = new FY3AVirrL1DataProvider_Accessor(); // TODO: 初始化为适当的值
            PrivateObject param0 = new PrivateObject(provider); // TODO: 初始化为适当的值
            FY3AVirrL1DataProvider_Accessor target = new FY3AVirrL1DataProvider_Accessor(param0); // TODO: 初始化为适当的值
            target.L1File = TestDataDir + "\\FY3A_VIRRX_GBAL_L1_20090427_0255_1000M_MS.HDF";
            target.ProvinceShpFile = TestDataDir+"\\province.shp";
           // target.FixLonLatBox();
            
            bool ret = target.CreateBorder();

            Assert.AreEqual(ret, true);
            //Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///FixLonLatBox 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("FireMonitor.exe")]
        public void FixLonLatBoxTest()
        {
            FY3AVirrL1DataProvider_Accessor provider = new FY3AVirrL1DataProvider_Accessor(); // TODO: 初始化为适当的值
            PrivateObject param0 = new PrivateObject(provider); // TODO: 初始化为适当的值
            FY3AVirrL1DataProvider_Accessor target = new FY3AVirrL1DataProvider_Accessor(param0); // TODO: 初始化为适当的值
            target.L1File = TestDataDir + "\\FY3A_VIRRX_GBAL_L1_20090427_0255_1000M_MS.HDF";
            RectangleF expected = new RectangleF((float)98.94003, (float)30.10315, (float)39.72882, (float)21.90414); // TODO: 初始化为适当的值
            RectangleF actual;
            actual = target.FixLonLatBox();

            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
