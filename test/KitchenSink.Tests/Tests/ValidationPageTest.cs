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
    public class ValidationPageTest : BaseTest
    {
        public ValidationPageTest(string browser) : base(browser)
        {
        }

        [Test]
        public void ValidationPage_PageLoads()
        {
            driver.Navigate().GoToUrl(baseURL + "/Validation");
        }

        [Test]
        public void ValidationPage_InvalidRequireInput()
        {
            driver.Navigate().GoToUrl(baseURL + "/Validation");
            var label = driver.FindElement(ByHelper.AnyErrorLabel);
            var originalText = label.Text;
            Assert.AreEqual(string.Empty, originalText);
            driver.FindElement(ByHelper.AnyButton).ClickUsingMouse(driver);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("'Name' should not be empty.", driver.FindElement(ByHelper.AnyErrorLabel).Text);
        }

        [Test]
        public void ValidationPage_ValidRequireInput()
        {
            driver.Navigate().GoToUrl(baseURL + "/Validation");
            var label = driver.FindElement(ByHelper.AnyErrorLabel);
            var originalText = label.Text;
            Assert.AreEqual(string.Empty, originalText);
            driver.FindElement(ByHelper.AnyInput).Clear();
            driver.FindElement(ByHelper.AnyInput).SendKeys("Marcin");
            this.WaitUntil(d => d.FindElement(ByHelper.AnyInput).GetAttribute("value") != string.Empty);
            driver.FindElement(ByHelper.AnyButton).ClickUsingMouse(driver);
            Assert.AreEqual(string.Empty, driver.FindElement(ByHelper.AnyErrorLabel).Text);
        }
    }
}