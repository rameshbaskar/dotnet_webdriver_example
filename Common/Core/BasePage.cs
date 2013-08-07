using System;
using Common.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Common.Core
{
    public class BasePage
    {
        protected static string CurrentWindow { get; set; }

        protected static IWebDriver Driver
        {
            get { return WebDriver.Driver; }
        }

        protected bool WaitForElementDisplayed(By element)
        {
            var clock = new SystemClock();
            var webDriverWait = new WebDriverWait(clock, Driver, new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 1));
            try
            {
                webDriverWait.Until(delegate { return IsDisplayed(element); });
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsDisplayed(By element)
        {
            try
            {
                if (Driver.FindElement(element).Displayed)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        protected static void SwitchToFrame(string frameName)
        {
            Driver.SwitchTo().Frame(frameName);
        }

        protected static void SwitchToDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }
    }
}