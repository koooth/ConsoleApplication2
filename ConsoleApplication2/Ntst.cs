using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;

namespace ConsoleApplication2
{
    class Ntst
   { 
        IWebDriver driver;

        [SetUp]
        public void Init()
        {
            driver = new FirefoxDriver();
        }

        [Test]
        public void JBJS()
        {
            driver.Url = "http://stage-journals.lww.com/jbjsoa/pages/results.aspx?txtkeywords=blood";
            System.Diagnostics.Debug.WriteLine("--------------------1");
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(50));
            IWebElement result0 = driver.FindElement(By.CssSelector(".resultCount"));
            System.Diagnostics.Debug.WriteLine("----------------------2");
            Assert.AreEqual("0 results", result0.Text);
            System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!" + result0.Text);
            System.Diagnostics.Debug.WriteLine("2nd" + result0.Text);
        }

        [TearDown]
        public void Clear()
        {
            driver.Close();
        }
    }
}
