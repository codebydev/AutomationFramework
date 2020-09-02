using AutomationFramework.TestBase;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Utilities
{
    class EventFiringWebDriverUtility
    {
        static ExtentTest test;
        public static EventFiringWebDriver GetEventFiringWebDriver(IWebDriver driver, ExtentTest test)
        {
            EventFiringWebDriverUtility.test = test;
            EventFiringWebDriver eventFiringWebDriver = new EventFiringWebDriver(driver);

            eventFiringWebDriver.ElementClicking += EventFiringWebDriver_ElementClicking;
            
            eventFiringWebDriver.ElementValueChanging += EventFiringWebDriver_ElementValueChanging;

            eventFiringWebDriver.ExceptionThrown += EventFiringWebDriver_ExceptionThrown;

            return eventFiringWebDriver;
        }

        
        private static void EventFiringWebDriver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            test.Log(LogStatus.Info, "Type : " + e.ThrownException.GetType());
            test.Log(LogStatus.Info, "Message : " + e.ThrownException.Message);
            test.Log(LogStatus.Info, "Source : " + e.ThrownException.Source);
            test.Log(LogStatus.Info, "StackTrace : " + e.ThrownException.StackTrace);            
        }

        private static void EventFiringWebDriver_ElementValueChanging(object sender, WebElementValueEventArgs e)
        {
            TestCasesBase.logs.Info("Value changing : " + e.Element.TagName);
            Console.WriteLine("Value changing : " + e.Element.TagName);
        }

        private static void EventFiringWebDriver_ElementClicking(object sender, WebElementEventArgs e)
        {
            TestCasesBase.logs.Info("Element clicking : " + e.Element.TagName);
            Console.WriteLine("Element clicking : " + e.Element.TagName);
        }        
    }
}
