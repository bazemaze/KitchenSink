using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KitchenSink.Tests;

namespace KitchenSink.Test
{
    [TestFixture("firefox")]
    [TestFixture("chrome")]
    [TestFixture("edge")]
    public class ButtonPageTest : BaseTest
    {
        public ButtonPageTest(string browser) : base(browser)
        {
        }

        [Test]
        public void ButtonPageTest_PageLoads()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(ByHelper.AnyLinkWithText("Button")).ClickUsingMouse(driver);
            var element = driver.FindElement(ByHelper.ButtonAddCarrotsInlineScript);
            Assert.AreEqual(element.Text.ToLower(), "button (inline script)");
        }

        [Test]
        public void ButtonPageTest_AddCarrotsIncrements()
        {
            driver.Navigate().GoToUrl(baseURL + "/Button");
            var label = driver.FindElement(ByHelper.AddCarrotsReaction);
            Assert.AreEqual("You don't have any carrots", label.Text);
            var originalText = label.Text;

            Click(driver.FindElement(ByHelper.ButtonAddCarrotsInlineScript));
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 1 imaginary carrots", label.Text);
            originalText = label.Text;

            Click(driver.FindElement(ByHelper.ButtonAddCarrotsFunction));
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 2 imaginary carrots", label.Text);
            originalText = label.Text;

            Click(driver.FindElement(ByHelper.SpanAddCarrotsFunction));
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 3 imaginary carrots", label.Text);
        }

        [Test]
        public void ButtonPageTest_SwitchButtonToggles()
        {
            driver.Navigate().GoToUrl(baseURL + "/Button");
            var button = driver.FindElement(ByHelper.ButtonSwitch);

            var label = driver.FindElement(ByHelper.ButtonSwitchReaction);
            Assert.AreEqual("Carrot engine is off", label.Text);
            var originalText = label.Text;

            Click(button);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("Carrot engine is on", label.Text);
            originalText = label.Text;

            Click(button);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("Carrot engine is off", label.Text);
        }

        [Test]
        public void ButtonPageTest_DisabledButton()
        {
            if (browser == "firefox")
            {
                Assert.Ignore("Click on disabled button is not supported in Selenium 3.0.0-beta2 in Firefox");
            }

            driver.Navigate().GoToUrl(baseURL + "/Button");
            var button = driver.FindElement(ByHelper.ButtonDisabled);
            var label = driver.FindElement(ByHelper.ButtonDisabledReaction);

            Assert.AreEqual("You don't have any carrots", label.Text);
            Assert.AreEqual(button.GetAttribute("disabled"), null);
            var originalText = label.Text;

            Click(button);
            this.WaitUntil(x => !label.Text.Equals(originalText));
            Assert.AreEqual("You have 1 imaginary carrots", label.Text);
            Assert.AreEqual(button.GetAttribute("disabled"), "true");

            Click(button);
            Thread.Sleep(1000);
            Assert.AreEqual("You have 1 imaginary carrots", label.Text);
            Assert.AreEqual(button.GetAttribute("disabled"), "true");
        }

        private void Click(IWebElement element)
        {
            element.ClickUsingMouse(driver);
        }
    }
}