using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.Support.UI;
using NLog;
using System.Drawing.Imaging;
using NUnit.Framework.Interfaces;
//using Excel = Microsoft.Office.Interop.Excel;

//[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace ConsoleApplication2
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }


    [TestFixture]
    class Ntst
   {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<TestCaseData> TS
        {
            get
            {
                var testCases = new List<TestCaseData>();

                using (var fs = File.OpenRead(@"C:\Users\Vadzim_Uladyka\Desktop\tst.csv"))
                using (var sr = new StreamReader(fs))
                {
                    string line = string.Empty;
                    while (line != null)
                    {
                        line = sr.ReadLine();
                        if (line != null)
                        {
                            string[] split = line.Split(new char[] { ',' },
                                StringSplitOptions.None);

                            string jrn = Convert.ToString(split[0]);

                            var testCase = new TestCaseData(jrn);
                            testCases.Add(testCase);
                        }
                    }
                }

                return testCases;
            }
        }

        IWebDriver driver;

        //public TestContext TestContext { get; set; }

        [OneTimeSetUp]
        public void Init()
        {

            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            //driver = new FirefoxDriver();
        }

        [TestCaseSource("TS")]
        public void nullsearch(string jrn)
        {
            //try
            //{
                driver.Navigate().GoToUrl("http://journals.lww.com/" + jrn + "/pages/results.aspx?txtkeywords=gggasdqwe");
                IWebElement result0 = driver.FindElement(By.CssSelector(".resultCount"), 30);
                IWebElement copyright = driver.FindElement(By.CssSelector(".copy"), 30);
                Assert.AreEqual("0 results", result0.Text);
                logger.Info(jrn);
                logger.Info("-------------------------" + result0.Text);
                logger.Info("-----------------------------------" + copyright.Text);
            //}
            //catch (Exception e)
            //{
            //    logger.Info("!!!!!Test Failed!!!!! - " + jrn);
            //}
        }

        //[TestCaseSource("TS")]
        //public void copyright(string jrn)
        //{
        //    driver.Url = "http://journals.lww.com/" + jrn;
        //    //driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(50));
        //    IWebElement copyright = new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.((By.CssSelector(".copy"))));
        //    //driver.FindElement(By.CssSelector(".copy"));
        //    Assert.IsNotEmpty(copyright.Text);
        //    Trace.WriteLine(jrn + " - " + copyright.Text);
        //    TestContext.WriteLine(jrn + " - " + copyright.Text);
        //    Debug.WriteLine(jrn + " - " + copyright.Text);
        //}
        [TearDown]
        public void screen()
        {
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string filename = "C:\\scr\\" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png";
                string now = DateTime.Now.ToString("ddMMyyy_hhmmss");
                string FileN = now + ".jpg";
                string path = "C:\\scr\\";
                screenshot.SaveAsFile(path + FileN, ImageFormat.Jpeg);
            }
        }


        [OneTimeTearDown]
        public void Clear()
        {
            driver.Close();
        }
    }
}