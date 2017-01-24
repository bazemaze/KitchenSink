using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KitchenSink.Tests;
using System.Threading;

namespace KitchenSink.Test
{
    [TestFixture("firefox")]
    [TestFixture("chrome")]
    [TestFixture("edge")]
    public class TextareaPageTest : BaseTest
    {
        public TextareaPageTest(string browser) : base(browser)
        {
        }

        /// <summary>
        /// TextareaPage_PageLoads is a which loads Textarea page from the Textarea-link
        /// </summary>
        [Test]
        public void TextareaPage_PageLoads()
        {
            driver.Navigate().GoToUrl(baseURL);

            driver.FindElement(ByHelper.AnyLinkWithText("Textarea")).ClickUsingMouse(driver);

            var element = driver.FindElement(ByHelper.AnyTextareaFormControl);
            Assert.AreEqual(element.Text, "");
        }


        /// <summary>
        /// TextareaPage_WriteToTextArea tests if it possible to write/read to/from the textarea component
        /// </summary>
        [Test]
        public void TextareaPage_WriteToTextArea()
        {
            if (browser == "firefox")
            {
                Assert.Ignore("GetAttribute(\"value\") is not supported in Selenium 3.0.0-beta2 in Firefox");
            }

            driver.Navigate().GoToUrl(baseURL + "/Textarea");

            driver.FindElement(ByHelper.AnyTextareaFormControl).Clear();

            string setString = "We all love princess cake!";
            driver.FindElement(ByHelper.AnyTextareaFormControl).SendKeys(setString);

            string actualString = driver.FindElement(ByHelper.AnyTextareaFormControl).GetAttribute("value");

            Assert.AreEqual(setString, actualString);
        }

        /// <summary>
        /// TextareaPage_CounterPropagationWhileTyping tests if the label continuously updates as the textarea changes
        /// </summary>
        [Test]
        public void TextareaPage_CounterPropagationWhileTyping()
        {
            driver.Navigate().GoToUrl(baseURL + "/Textarea");

            var label = driver.FindElement(ByHelper.AnyControlLabel);
            var originalText = label.Text;

            driver.FindElement(ByHelper.AnyTextarea).Clear();
            Assert.AreEqual("Length of your bio: 0 chars", originalText);

            driver.FindElement(ByHelper.AnyTextarea).SendKeys("U");
            this.WaitUntil(x => !label.Text.Equals(originalText));

            string actualString = driver.FindElement(ByHelper.AnyControlLabel).Text;
            Assert.AreEqual("Length of your bio: 1 chars", actualString);
        }
    }
}