using System;
using System.Text;
using System.IO;
using Microsoft.Win32;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace KitchenSink.Tests
{
    public class BaseTest
    {
        public BaseTest(string browser)
        {
            this.browser = browser;
        }

        protected IWebDriver driver;
        protected StringBuilder verificationErrors;
        protected string baseURL;
        protected string browser;
        protected WebDriverWait wait;

        [SetUp]
        public void SetupTest()
        {
            baseURL = "http://localhost:8080/KitchenSink";
            verificationErrors = new StringBuilder();

            if (browser == "edge")
            {
                Assert.Ignore("Edge tests are disabled due to random timeout fails.");
            }

            if (browser == "edge" && !IsEdgeAvailable())
            {
                Assert.Ignore(
                    "You're not using Windows 10, so Microsoft Edge is unavailable. The test is being omitted.");
            }

            this.driver = WebDriverFactory.Create(this.browser);
            this.wait = this.GetWebDriverWait();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }

            Assert.AreEqual(string.Empty, verificationErrors.ToString());
        }

        protected bool IsEdgeAvailable()
        {
            var browserKeys =
                Registry.ClassesRoot.OpenSubKey(
                    @"Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Packages");
            if (browserKeys == null)
            {
                return false;
            }
            else
            {
                var elements = browserKeys.GetSubKeyNames();
                foreach (var element in elements)
                {
                    if (element.Contains("MicrosoftEdge"))
                    {
                        var keyPath = browserKeys.OpenSubKey(element);
                        var edgePath = keyPath.GetValue("Path");
                        if (edgePath != null && Directory.Exists(edgePath.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        protected WebDriverWait GetWebDriverWait()
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        protected TResult WaitUntil<TResult>(Func<IWebDriver, TResult> condition)
        {
            return this.wait.Until(condition);
        }
    }
}