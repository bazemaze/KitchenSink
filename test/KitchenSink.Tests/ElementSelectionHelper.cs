using OpenQA.Selenium;

namespace KitchenSink.Tests
{
    public static class ByHelper
    {
        public static By PuppetClient
        {
            get { return By.CssSelector("html body puppet-client"); }
        }

        public static By StarcounterIncludeWithDiv
        {
            get
            {
                return
                    By.CssSelector(
                        "div.kitchensink-layout__column-right > starcounter-include > juicy-composition > div");
            }
        }

        public static By StarcounterIncludeWithCheckbox
        {
            get
            {
                return
                    By.CssSelector(
                        "div.kitchensink-layout__column-right > starcounter-include > juicy-composition > input[type='checkbox']");
            }
        }

        public static By StarcounterIncludeWithPassword
        {
            get
            {
                return
                    By.CssSelector(
                        "div.kitchensink-layout__column-right > starcounter-include > juicy-composition > input[type='password']");
            }
        }

        public static By StarcounterIncludeWithInputText
        {
            get
            {
                return
                    By.CssSelector(
                        "div.kitchensink-layout__column-right > starcounter-include > juicy-composition > input[type='text']");
            }
        }

        public static By StarcounterIncludeWithTextarea3Rows
        {
            get
            {
                return
                    By.CssSelector(
                        "div.kitchensink-layout__column-right > starcounter-include > juicy-composition > textarea[rows='3']");
            }
        }

        public static By Body
        {
            get { return By.XPath("//body"); }
        }

        public static By AnyInput
        {
            get { return By.XPath("(//input)[1]"); }
        }

        public static By NthInput(int count)
        {
            count += 1;
            return By.XPath("(//input)[" + count + "]");
        }

        public static By AnyTextarea
        {
            get { return By.XPath("(//textarea)[1]"); }
        }

        public static By AnyTextareaFormControl
        {
            get { return By.XPath("(//textarea[@class='form-control'])[1]"); }
        }

        public static By AnyControlLabel
        {
            get { return By.XPath("(//label[@class='control-label'])[1]"); }
        }

        public static By NthControlLabel(int count)
        {
            count += 1;
            return By.XPath("(//label[@class='control-label'])[" + count + "]");
        }

        public static By AnyErrorLabel
        {
            get { return By.XPath("(//label[@class='error-label'])[1]"); }
        }

        public static By AnyButton
        {
            get { return By.XPath("//button"); }
        }

        public static By AnyButtonWithText(string text)
        {
            text = text.Replace("\"", "\\\"");
            return By.XPath("//button[text()=\"" + text + "\"]");
        }

        public static By AnyDivWithText(string text)
        {
            text = text.Replace("\"", "\\\"");
            return By.XPath("//div[text()=\"" + text + "\"]");
        }

        public static By AnyLinkWithText(string text)
        {
            text = text.Replace("\"", "\\\"");
            return By.LinkText(text);
        }

        public static By ButtonAddCarrotsInlineScript
        {
            get { return By.XPath("(//p[@class='kitchensink-add-carrots']/button)[1]"); }
        }

        public static By ButtonAddCarrotsFunction
        {
            get { return By.XPath("(//p[@class='kitchensink-add-carrots']/button)[2]"); }
        }

        public static By SpanAddCarrotsFunction
        {
            get { return By.XPath("(//p[@class='kitchensink-add-carrots']/span)[2]"); }
        }

        public static By AddCarrotsReaction
        {
            get { return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include//pre)[1]"); }
        }

        public static By ButtonSwitch
        {
            get
            {
                return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[3]");
            }
        }

        public static By ButtonSwitchReaction
        {
            get
            {
                return
                    By.XPath(
                        "(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[3]/following-sibling::span[1]");
            }
        }

        public static By ButtonDisabled
        {
            get
            {
                return By.XPath("(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[4]");
            }
        }

        public static By ButtonDisabledReaction
        {
            get
            {
                return
                    By.XPath(
                        "(//div[@class='kitchensink-layout__column-right']/starcounter-include//button)[4]/following-sibling::span[1]");
            }
        }
    }
}