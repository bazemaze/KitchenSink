using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

namespace KitchenSink.Tests
{
    public static class SeleniumExtensions
    {
        /// <summary>
        /// Click on the element using Actions API. 'Click' method is known to not work in same strange cases on Edge,
        /// Use this method instead
        /// </summary>
        public static void ClickUsingMouse(this IWebElement element, IWebDriver driver)
        {
            ICapabilities capabilities = ((RemoteWebDriver) driver).Capabilities;
            if (capabilities.BrowserName.ToLower() == "firefox")
            {
                element.Click();
            }
            else
            {
                new Actions(driver).Click(element).Build().Perform();
                    //does not work with Firefox 48, Selenium 3.0.0-beta2, GeckoDriver.exe 0.10.0
            }
        }
    }
}