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
    public class CheckboxPageTest : BaseTest
    {
        private string canDrive = "You can drive";
        private string cantDrive = "You can't drive";

        public CheckboxPageTest(string browser) : base(browser)
        {
        }

        [Test]
        public void CheckboxPage_PageLoads()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(ByHelper.AnyLinkWithText("Checkbox")).ClickUsingMouse(driver);
            var element = driver.FindElement(ByHelper.AnyInput);
            Assert.AreEqual(element.GetAttribute("type"), "checkbox");
        }

        [Test]
        public void CheckboxPage_CheckboxUnchecked()
        {
            driver.Navigate().GoToUrl(baseURL + "/Checkbox");
            var element = driver.FindElement(ByHelper.AnyInput);
            Assert.AreEqual(element.GetAttribute("type"), "checkbox");
            element.ClickUsingMouse(driver);
            this.WaitUntil(x => !canDrive.Equals(driver.FindElement(ByHelper.StarcounterIncludeWithDiv).Text));
            var uncheckedText = driver.FindElement(ByHelper.StarcounterIncludeWithDiv).Text;
            Assert.AreEqual(cantDrive, uncheckedText);
        }

        [Test]
        public void CheckboxPage_CheckboxUncheckedAndCheckedAgain()
        {
            driver.Navigate().GoToUrl(baseURL + "/Checkbox");
            var element = driver.FindElement(ByHelper.AnyInput);
            Assert.AreEqual(element.GetAttribute("type"), "checkbox");
            element.ClickUsingMouse(driver);
            this.WaitUntil(x => !canDrive.Equals(driver.FindElement(ByHelper.StarcounterIncludeWithDiv).Text));
            var uncheckedText = driver.FindElement(ByHelper.StarcounterIncludeWithDiv).Text;
            Assert.AreEqual(cantDrive, uncheckedText);
            element.ClickUsingMouse(driver);
            this.WaitUntil(x => !cantDrive.Equals(driver.FindElement(ByHelper.StarcounterIncludeWithDiv).Text));
            var checkedText = driver.FindElement(ByHelper.StarcounterIncludeWithDiv).Text;
            Assert.AreEqual(canDrive, checkedText);
        }
    }
}