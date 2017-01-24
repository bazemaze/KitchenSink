using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KitchenSink.Tests;

namespace KitchenSink.Test
{
    [TestFixture("firefox")]
    [TestFixture("chrome")]
    [TestFixture("edge")]
    //[TestFixture("internet explorer")]
    public class PasswordPageTest : BaseTest
    {
        private string passwordTooShort = "Password must be at least 6 chars long";
        private string passwordWithProperLength = "Good password!";

        public PasswordPageTest(string browser) : base(browser)
        {
        }

        [Test]
        public void PasswordPage_PageLoads()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(ByHelper.AnyLinkWithText("Password")).ClickUsingMouse(driver);
            var element = driver.FindElement(ByHelper.AnyInput);
            Assert.AreEqual(element.GetAttribute("type"), "password");
        }

        [Test]
        public void PasswordPage_PasswordTooShort()
        {
            driver.Navigate().GoToUrl(baseURL + "/Password");
            var element = driver.FindElement(ByHelper.AnyInput);
            Assert.AreEqual(element.GetAttribute("type"), "password");
            driver.FindElement(ByHelper.AnyInput).Clear();
            driver.FindElement(ByHelper.AnyInput).SendKeys("Short");
            Assert.AreEqual(passwordTooShort, driver.FindElement(ByHelper.AnyControlLabel).Text);
        }

        [Test]
        public void PasswordPage_PasswordWithProperLength()
        {
            driver.Navigate().GoToUrl(baseURL + "/Password");
            var element = driver.FindElement(ByHelper.AnyInput);
            Assert.AreEqual(element.GetAttribute("type"), "password");
            driver.FindElement(ByHelper.AnyInput).Clear();
            driver.FindElement(ByHelper.AnyInput).SendKeys("PerfectPass");
            this.WaitUntil(x => !passwordTooShort.Equals(driver.FindElement(ByHelper.AnyControlLabel).Text));
            Assert.AreEqual(passwordWithProperLength, driver.FindElement(ByHelper.AnyControlLabel).Text);
        }

        [Test]
        public void PasswordPage_ChangingPasswordToGoodThenToShort()
        {
            driver.Navigate().GoToUrl(baseURL + "/Password");
            var element = driver.FindElement(ByHelper.AnyInput);
            Assert.AreEqual(element.GetAttribute("type"), "password");
            driver.FindElement(ByHelper.AnyInput).Clear();
            driver.FindElement(ByHelper.AnyInput).SendKeys("PerfectPass");
            this.WaitUntil(x => !passwordTooShort.Equals(driver.FindElement(ByHelper.AnyControlLabel).Text));
            driver.FindElement(ByHelper.AnyInput).Clear();
            driver.FindElement(ByHelper.AnyInput).SendKeys("Bad");
            this.WaitUntil(x => !passwordWithProperLength.Equals(driver.FindElement(ByHelper.AnyControlLabel).Text));
            Assert.AreEqual(passwordTooShort, driver.FindElement(ByHelper.AnyControlLabel).Text);
        }
    }
}