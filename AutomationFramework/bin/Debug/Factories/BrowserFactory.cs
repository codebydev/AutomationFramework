using AutomationFramework.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using RelevantCodes.ExtentReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Factories
{
    class BrowserFactory
    {
        public static IWebDriver GetBrowser(ExtentTest test)
        {
            IWebDriver driver = null;
            string browser = ConfigurationManager.AppSettings["browser"];
            string url = ConfigurationManager.AppSettings["url"];

            switch (browser)
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "ie":
                    driver = new InternetExplorerDriver();
                    break;
                case "edge":
                    driver = new EdgeDriver();
                    break;
                default:
                    driver = new ChromeDriver();
                    break;
            }
            driver.Manage().Window.Maximize();
            driver = EventFiringWebDriverUtility.GetEventFiringWebDriver(driver, test);
            driver.Url = url;
            return driver;
        }
    }
}
