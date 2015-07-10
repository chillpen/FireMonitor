using MapHandle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SharpMap.Forms;
using System.Windows.Forms;
using SharpMap.Layers;
using SharpMap;

namespace TestProject
{


    /// <summary>
    ///这是 ImgDispCtrlTest 的测试类，旨在
    ///包含所有 ImgDispCtrlTest 单元测试
    ///</summary>
    [TestClass()]
    public class ImgDispCtrlTest
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
        ///ImgDispCtrl 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void ImgDispCtrlConstructorTest()
        {
            ImgDispCtrl target = new ImgDispCtrl();
            // Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///Dispose 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MapHandle.dll")]
        public void DisposeTest()
        {
            ImgDispCtrl_Accessor target = new ImgDispCtrl_Accessor(); // TODO: 初始化为适当的值
            bool disposing = false; // TODO: 初始化为适当的值
            target.Dispose(disposing);
            // Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///InitializeComponent 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MapHandle.dll")]
        public void InitializeComponentTest()
        {
            ImgDispCtrl_Accessor target = new ImgDispCtrl_Accessor(); // TODO: 初始化为适当的值
            target.InitializeComponent();
            //Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///MapBox 的测试
        ///</summary>
        [TestMethod()]
        public void MapBoxTest()
        {
            ImgDispCtrlForm form = new ImgDispCtrlForm();
            ImgDispCtrl target = form.ImgDispCtrl; // TODO: 初始化为适当的值

            MapBox actual;
            actual = target.MapBox;




            GdiImageLayer gdilayer = new GdiImageLayer("C:\\Chrysanthemum.jpg");
            actual.Map.Layers.Add(gdilayer);
            //actual.Map.ZoomToBox(gdilayer.Envelope);

            actual.Map.ZoomToExtents();
            actual.Refresh();


            Application.Run(form);


            // Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
