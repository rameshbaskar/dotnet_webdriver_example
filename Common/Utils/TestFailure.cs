using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Common.Driver;
using OpenQA.Selenium;

namespace Common.Utils
{
    public class TestFailure
    {
        [ThreadStatic]
        private static List<string> failures;

        private static List<string> Failures
        {
            get
            {
                return failures ?? (failures = new List<string>());
            }
        }

        private static bool TestFailed
        {
            get
            {
                return Failures.Count > 0;
            }
        }

        private static string GetFailureMessages()
        {
            var newLine = Environment.NewLine;
            var failureMessages = newLine + string.Format("Total number of assertions failures: {0}", Failures.Count)
                                  + newLine + newLine;

            failureMessages = Failures.Aggregate(failureMessages, (current, s) => current + (s + newLine));

            return failureMessages + newLine;
        }

        // This would be called during [SetUp]
        public static void ClearFailures()
        {
            Failures.Clear();
        }

        // This would be called during [TearDown]
        public static void LogFailureMessagesAndFail()
        {
            if (!TestFailed)
            {
                return;
            }

            string failureMessages = null;
            try
            {
                failureMessages = GetFailureMessages();
            }
            catch (Exception e)
            {
                Verify.Fail("Failure while logging errors: " + e);
            }
            Logger.Info("Failed with the following message..." + Environment.NewLine);
            Logger.Info(failureMessages);
            Verify.Fail(failureMessages);
        }

        public static void HandleAssertionException(Exception ae, bool bContinue)
        {
            var exceptionLog = new StringBuilder();

            exceptionLog.AppendLine(DateTime.Now.ToString());
            exceptionLog.AppendLine(ae.Message);

            var screenShotFullPath = TestEnvironment.ScreenShotPath + (Failures.Count + 1) + ".png";
            exceptionLog.Append("ScreenShot Path: " + screenShotFullPath + Environment.NewLine + Environment.NewLine);

            var pageSourceFullPath = TestEnvironment.FileDumpPath + (Failures.Count + 1) + ".html";
            exceptionLog.Append("PageSource Path: " + pageSourceFullPath + Environment.NewLine + Environment.NewLine);

            exceptionLog.Append(GetStackTrace());

            Failures.Add(exceptionLog.ToString());

            CaptureScreenShot(screenShotFullPath);
            CapturePageSource(pageSourceFullPath);

            Logger.Info(Environment.NewLine + "Following exception occured..." + Environment.NewLine);
            Logger.Info(exceptionLog.ToString());
            
            if (!bContinue)
            {
                throw new Exception(
                    Environment.NewLine + "Test execution halted with below errors:" + Environment.NewLine);
            }
        }

        public static void CaptureScreenShot(string screenShotFullPath)
        {
            var directory = Path.GetDirectoryName(screenShotFullPath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var driver = DriverProvider.Driver as ITakesScreenshot;
            var screenshot = driver.GetScreenshot();

            File.Delete(screenShotFullPath);
            screenshot.SaveAsFile(screenShotFullPath, ImageFormat.Png);
        }

        public static StringBuilder GetStackTrace()
        {
            var stackTrace = new StackTrace(true);
            var traceAsString = new StringBuilder();

            var myOwnCodeFileName = stackTrace.GetFrame(0).GetFileName();
            foreach (var stackFrame in stackTrace.GetFrames())
            {
                if ((0 == stackFrame.GetFileLineNumber()) || myOwnCodeFileName.Equals(stackFrame.GetFileName()))
                {
                    continue;
                }

                var method = stackFrame.GetMethod();
                traceAsString.AppendLine(
                    "at " + method.DeclaringType.FullName + "." + method.Name + @" in "
                    + new FileInfo(stackFrame.GetFileName()).Name + ":line " + stackFrame.GetFileLineNumber());
            }

            return traceAsString;
        }

        public static void CapturePageSource(string pageSourceFullPath)
        {
            var directory = Path.GetDirectoryName(pageSourceFullPath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var driver = DriverProvider.Driver;
            var pagesource = driver.PageSource;

            File.Delete(pageSourceFullPath);
            File.WriteAllText(pageSourceFullPath, pagesource);
        }
    }
}