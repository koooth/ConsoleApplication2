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

        public static IList<IWebElement> elements;

        [OneTimeSetUp]
        public void Init()
        {
            logger.Info("|----------------------------------------------------------------------------------|");
            logger.Info("|-----------------------------------Test started-----------------------------------|");
            logger.Info("|----------------------------------------------------------------------------------|");
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            //driver = new FirefoxDriver();
        }

        [TestCaseSource("TS")]
        public void nullsearch(string jrn)
        {
            //try
            //{
   
                driver.Navigate().GoToUrl("http://stage-journals.lww.com/" + jrn + "/pages/results.aspx?txtkeywords=gggasdqwe");
                IWebElement result0 = driver.FindElement(By.CssSelector(".resultCount"), 15);
                IWebElement copyright = driver.FindElement(By.CssSelector(".copy"), 15);
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

        [Test]
        public void UnExpectedElement()
        {
            //driver.Navigate().GoToUrl("http://journals.lww.com/plasreconsurg/Fulltext/2017/01000/Late_Surgical_Site_Infection_in_Immediate.5.aspx");
            //System.Threading.Thread.Sleep(10000);
            //IWebElement code = driver.FindElement(By.TagName("body"), 30);
            //Assert.IsTrue(code.Text.Contains("&#x"));
            //Assert.IsTrue(code.Text.Contains("&#x201D;"));
            driver.Navigate().GoToUrl("http://journals.lww.com/plasreconsurg/toc/2017/01000/"); ///Pages/currenttoc.aspx
            IWebElement list = driver.FindElement(By.CssSelector(".article-list"), 30);
            elements = list.FindElements(By.TagName("article"));
            int count = elements.Count;
            logger.Info("articles on page = " + count);
            for (int i = 1; i <= count; i = i + 1)
            {
                driver.Navigate().GoToUrl("http://journals.lww.com/plasreconsurg/FullText/2017/01000/Why_Some_Mastectomy_Patients_Opt_to_Undergo." + i +".aspx");
                IWebElement code = driver.FindElement(By.TagName("body"), 30);
                Assert.IsFalse(code.Text.Contains("&amp;#x"));
                logger.Info(i + "st article verified");
            }
            logger.Info(count + " totally articles verified");
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
                logger.Info("Test failed" + TestContext.CurrentContext.Test.Name);
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
            logger.Info("|----------------------------------------------------------------------------------|");
            logger.Info("|-----------------------------------Test finished----------------------------------|");
            logger.Info("|----------------------------------------------------------------------------------|");
        }
    }
}