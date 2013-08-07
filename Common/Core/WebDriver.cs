using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Common.Core
{
    public class WebDriver
    {
        [ThreadStatic]
        private static IWebDriver _driver;

        public static void InitializeDriver() 
        {
            _driver = new FirefoxDriver(new FirefoxProfile());
            timeout = ConfigurationManager.AppSettings["implicit.wait.time"];
            _driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, int.Parse(timeout)));
        }

        public static IWebDriver Driver
        {
            get { return _driver; }
        }
    }
}