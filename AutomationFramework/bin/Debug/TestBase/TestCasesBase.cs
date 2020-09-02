using AutomationFramework.Extensions;
using AutomationFramework.Factories;
using AutomationFramework.Utilities;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using RelevantCodes.ExtentReports;
using System;
using WDSE;
using WDSE.Decorators;
using WDSE.ScreenshotMaker;

namespace AutomationFramework.TestBase
{
    [TestClass]
    public class TestCasesBase
    {
        public static IWebDriver driver;
        public static ILog logs = LogManager.GetLogger(typeof(TestCasesBase));
        public static ExtentReports extentReports;
        public static string reportFileName;
        public ExtentTest test;
        public WebElementExtensions webElementExtensions;
        public WebDriverExtensions webDriverExtensions;

        public TestContext TestContext { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            reportFileName = "Test report" + DateTime.Now.ToString("yyyy_MM__dd__hhmm") + ".html";
            extentReports = new ExtentReports(reportFileName);
            extentReports.LoadConfig("extentReportConfiguration.xml");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            extentReports.Flush();
            Mailer.SendMail("ankprotraining@gmail.com;sudheer.c26@gmail.com", "SauceLabs Automation Regression Report: " + DateTime.Now.ToShortDateString(), "<b>Saucelabs automation report</b>", reportFileName);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            logs.Info("===============================================");
            logs.Info(TestContext.TestName + " Test case is getting executed");
            test = extentReports.StartTest(TestContext.TestName);
            driver = BrowserFactory.GetBrowser(test);
            webElementExtensions = new WebElementExtensions(driver);
            webDriverExtensions = new WebDriverExtensions(driver);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                //string str = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;

                var vcs = new VerticalCombineDecorator(new ScreenshotMaker().RemoveScrollBarsWhileShooting());
                string str = ((EventFiringWebDriver)driver).WrappedDriver.TakeScreenshot(vcs).ToMagickImage().ToBase64();

                str = "data:image/png;base64," + str;
                test.Log(LogStatus.Fail, "<img src=\"" + str + "\"</img>");
            }
            else
            {
                test.Log(LogStatus.Pass, "Test passed");
            }
            driver.Quit();
            extentReports.EndTest(test);
            logs.Info("Browser closed");
        }
        public void Log(LogStatus logStatus, string message)
        {
            test.Log(logStatus, message);
            logs.Info(message);
        }        
    }
}
