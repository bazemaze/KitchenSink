using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace KitchenSink.Tests.Tests
{
    [TestFixture("firefox")]
    [TestFixture("chrome")]
    [TestFixture("edge")]
    public class RedirectPageTest : BaseTest
    {
        public RedirectPageTest(string browser) : base(browser)
        {
        }

        [SetUp]
        public void Setup()
        {
            driver.Navigate().GoToUrl(baseURL + "/Redirect");
        }

        [Test]
        public void ClickingOnFruitShouldChangeUrlAndText()
        {
            ClickButton("Fruit");

            // on edge juicy sometimes messes up the dom tree, so you can't be sure about its relative position to button
            this.WaitUntil(ExpectedConditions.ElementIsVisible(ByHelper.AnyDivWithText("You've got some tasty apple")));
            Assert.That(driver.Url, Is.EqualTo($"{baseURL}/Redirect/apple"));
        }

        [Test]
        public void ClickingOnRedirectToAnotherPartialShouldChangeUrl()
        {
            ClickButton("Morph to another partial");

            // redirecting can take some time
            this.WaitUntil(ExpectedConditions.UrlContains(baseURL));
        }

        [Test]
        public void ClickingOnRedirectToExternalWebsiteShouldChangeUrl()
        {
            ClickButton("Redirect to Starcounter.io");

            // see https://github.com/PuppetJs/puppet-redirect/issues/3
            // this is no longer needed, since puppet-client shows a "reconnection" message instead of alert
            //if (browser == "firefox") {
            // depending on wheter or not Launcher will be present, the dialog will differ
            // _wait.Until(d => WaitForNoConnectionAndDismiss(d) || d.FindElements(By.XPath("//h4[text()='Connection error']")).Count != 0);
            //}

            // redirecting can take some time
            this.WaitUntil(ExpectedConditions.UrlContains("https://starcounter.io/"));
        }

        private bool WaitForNoConnectionAndDismiss(IWebDriver d)
        {
            try
            {
                d.SwitchTo().Alert().Dismiss();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private void ClickButton(string text)
        {
            driver.FindElement(ByHelper.AnyButtonWithText(text)).ClickUsingMouse(driver);
        }
    }
}