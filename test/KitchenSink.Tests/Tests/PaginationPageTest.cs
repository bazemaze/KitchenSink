using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KitchenSink.Tests;
using System.Threading;
using System.Collections.Generic;

namespace KitchenSink.Test
{
    [TestFixture("firefox")]
    [TestFixture("chrome")]
    [TestFixture("edge")]
    public class PaginationPageTest : BaseTest
    {
        public PaginationPageTest(string browser) : base(browser)
        {
        }

        [SetUp]
        public void SetUp()
        {
            driver.Navigate().GoToUrl(baseURL + "/pagination");
            this.WaitUntil(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("puppet-client")));
        }

        [Test]
        public void PaginationPage_PageLoads()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(ByHelper.AnyLinkWithText("Pagination")).ClickUsingMouse(driver);
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("kitchensink-pagination-entry")));
        }

        [Test]
        public void PaginationPage_Dropdown_HasCorrectOptions()
        {
            var dropDownOptions = driver.FindElements(By.XPath("//select/option"));
            string[] entriesPerPage = new string[] { "5", "15", "30" };
            for (int i = 0; i < dropDownOptions.Count; i++)
            {
                Assert.AreEqual(dropDownOptions[i].Text, entriesPerPage[i]);
            }
        }

        [Test]
        public void PaginationPage_DropDown_RightNumberOfElementsOnClick()
        {
            // The functionality tested in this test works in firefox 48, although this test fails
            // Remove this condition when teamcity is upgraded to firefox 49 where it passes
            if (browser == "firefox")
            {
                Assert.Ignore("Firefox 48 with Selenium 3.0.0-beta2 is not compatible for this test");
            }

            var dropDownOptions = driver.FindElements(By.XPath("//select/option"));
            int[] entriesPerPage = new int[] { 5, 15, 30 };
            for (int i = 0; i < dropDownOptions.Count; i++)
            {
                var dropDown = driver.FindElement(By.TagName("select"));
                var dropDownSelect = new SelectElement(dropDown);
                dropDownSelect.SelectByIndex(i);
                wait.Until(x => driver.FindElements(By.ClassName("kitchensink-pagination-entry")).Count == entriesPerPage[i]);
            }
        }

        [Test]
        public void PaginationPage_LastButton_GoesToLastPage()
        {
            driver.FindElement(By.Id("kitchensink-pagination-last")).ClickUsingMouse(driver);
            wait.Until(x => driver.FindElement(By.ClassName("kitchensink-pagination-entry")).Text == "Arbitrary Book 96 - Arbitrary Author");
        }

        [Test]
        public void PaginationPage_FirstButton_GoesToFirstPage()
        {
            PaginationPage_LastButton_GoesToLastPage();
            driver.FindElement(By.Id("kitchensink-pagination-first")).ClickUsingMouse(driver);
            wait.Until(x => driver.FindElement(By.ClassName("kitchensink-pagination-entry")).Text == "Arbitrary Book 1 - Arbitrary Author");
        }

        [Test]
        public void PaginationPage_NextButton_GoesToNextPage()
        {
            driver.FindElement(By.Id("kitchensink-pagination-next")).ClickUsingMouse(driver);
            wait.Until(x => driver.FindElement(By.ClassName("kitchensink-pagination-entry")).Text == "Arbitrary Book 6 - Arbitrary Author");
        }

        [Test]
        public void PaginationPage_PreviousButton_GoesToPreviousPage()
        {
            PaginationPage_NextButton_GoesToNextPage();
            driver.FindElement(By.Id("kitchensink-pagination-previous")).ClickUsingMouse(driver);
            wait.Until(x => driver.FindElement(By.ClassName("kitchensink-pagination-entry")).Text == "Arbitrary Book 1 - Arbitrary Author");
        }

        [Test]
        public void PaginationPage_NavButtons_GoesToRightPage()
        {
            var buttons = driver.FindElements(By.ClassName("kitchensink-pagination-change"));
            for (int i = 0; i < 3; i++)
            {
                buttons[i].ClickUsingMouse(driver);
                wait.Until(x => "Arbitrary Book " + (Int32.Parse(buttons[i].Text) * 5 - 4).ToString() + " - Arbitrary Author" == driver.FindElement(By.ClassName("kitchensink-pagination-entry")).Text);
            }

        }
    }
}