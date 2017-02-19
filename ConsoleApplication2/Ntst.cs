using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.Support.UI;
//using Excel = Microsoft.Office.Interop.Excel;


namespace ConsoleApplication2
{
    class Ntst
   {
        public static List<TestCaseData> TS
        {
            get
            {
                var testCases = new List<TestCaseData>();

                using (var fs = File.OpenRead(@"C:\tst.csv"))
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

        [SetUp]
        public void Init()
        {
            driver = new FirefoxDriver();
        }

        //[Test]
        //public void JBJS([Values("prsgo", "jbjsoa", "plasreconsurg")] string journal)
        //{
        //    driver.Url = "http://journals.lww.com/" + journal + "/pages/results.aspx?txtkeywords=ggg";
        //    //Debug.WriteLine("--------------------1");
        //    driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(50));
        //    IWebElement result0 = driver.FindElement(By.CssSelector(".resultCount"));
        //    //Debug.WriteLine("----------------------2");
        //    Assert.AreEqual("0 results", result0.Text);
        //    //Debug.WriteLine("!!!!!!!!!!!!!!!!!!!" + result0.Text);
        //    //Debug.WriteLine("2nd" + result0.Text);
        //    //Debug.WriteLine("3rd" + result0.Text);
        //    Trace.WriteLine(journal + " - " + result0.Text);
        //    TestContext.WriteLine(journal + " - " + result0.Text);
        //}

        [TestCaseSource("TS")]
        public void nullsearch(string jrn)
        {
            driver.Url = "http://journals.lww.com/" + jrn + "/pages/results.aspx?txtkeywords=gggasdqwe";
            //Debug.WriteLine("--------------------1");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(50));
            IWebElement result0 = driver.FindElement(By.CssSelector(".resultCount"));
            IWebElement copyright = driver.FindElement(By.CssSelector(".copy"));
            //IWebElement result0 = new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.((By.CssSelector(".resultCount"))));
            //IWebElement copyright = new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(ExpectedConditions.((By.CssSelector(".copy")));
            //Debug.WriteLine("----------------------2");
            Assert.AreEqual("0 results", result0.Text + " - " + copyright.Text);
            //Debug.WriteLine("!!!!!!!!!!!!!!!!!!!" + result0.Text);
            //Debug.WriteLine("2nd" + result0.Text);
            Debug.WriteLine(jrn + " - " + result0.Text + " - " + copyright.Text);
            Trace.WriteLine(jrn + " - " + result0.Text + " - " + copyright.Text);
            TestContext.WriteLine(jrn + " - " + result0.Text + " - " + copyright.Text);
        }

        //public static class WebDriverExtensions
        //{
        //    public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        //    {
        //        if (timeoutInSeconds > 0)
        //        {
        //            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
        //            return wait.Until(drv => drv.FindElement(by));
        //        }
        //        return driver.FindElement(by);
        //    }
        //}

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
        public void Clear()
        {
            driver.Close();
        }
    }
}