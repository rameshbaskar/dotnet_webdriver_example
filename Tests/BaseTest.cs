using System;
using System.Configuration;
using Common.Utils;
using Domain.Pages;

namespace Tests
{
    public class BaseTest
    {
        protected static HomePage HomePage = new HomePage();

        protected void AssemblySetUp()
        {
            WebDriver.InitializeDriver
        }

        protected void AssemblyTearDown()
        {
            if (WebDriver.Driver != null)
            {
                WebDriver.Driver.Quit();
            }
        }

        protected void FixtureSetup()
        {
            if (WebDriver.Driver == null)
            {
                InitializeDriver();
            }
        }

        protected void FixtureTearDown(bool closeBrowser)
        {
            if (WebDriver.Driver != null && closeBrowser)
            {
                WebDriver.Driver.Quit();
            }
        }

        protected void TestSetUp()
        {
            TestFailure.ClearFailures();
        }

        protected void TestTearDown()
        {
            TestFailure.LogFailureMessagesAndFail();
        }
    }
}