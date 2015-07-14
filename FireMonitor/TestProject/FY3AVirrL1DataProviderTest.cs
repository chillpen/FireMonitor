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
            target.File = "C:\\Data\\FY3A_VIRRX_GBAL_L1_20090427_0255_1000M_MS.HDF";
           // Bitmap expected = null; // TODO: 初始化为适当的值
           // Bitmap actual;
            //actual = target.GetData();
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");

            ImgDispCtrlForm form = new ImgDispCtrlForm();
            ImgDispCtrl imgDispCtrl = form.ImgDispCtrl; // TODO: 初始化为适当的值

            MapBox mapBox;
            mapBox = imgDispCtrl.MapBox;




            GdiImageLayer gdilayer = new GdiImageLayer("C:\\Chrysanthemum.jpg", target);
            mapBox.Map.Layers.Add(gdilayer);
            //actual.Map.ZoomToBox(gdilayer.Envelope);

            mapBox.Map.ZoomToExtents();
            mapBox.Refresh();


            Application.Run(form);
        }

        /// <summary>
        ///File 的测试
        ///</summary>
        [TestMethod()]
        public void FileTest()
        {
            FY3AVirrL1DataProvider target = new FY3AVirrL1DataProvider(); // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            target.File = expected;
           // Assert.Inconclusive("无法验证只写属性。");
        }
    }
}
