using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomationFramework.Extensions
{
    public class WebDriverExtensions
    {
        public IWebDriver driver;

        public WebDriverExtensions(IWebDriver driver) 
        {
            this.driver = driver;
        }

        public void PageLoadTimeout(string url)
        {
            driver.Manage().Timeouts().PageLoad.TotalSeconds.Equals(TimeSpan.FromSeconds(120));
            NavigateToSite(url);
        }

        public void NavigateToSite(string URL)
        {
            driver.Navigate().GoToUrl(URL);
        }

        public void WaitForPageToLoad()
        {
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            wait.Until(PageLoaded()).Equals("complete");
        }

        private Func<IWebDriver, string> PageLoaded()
        {
            return ((x) =>
            {
                return ExecuteScriptWithValueReturned("return document.readyState");
            });
        }

        private string ExecuteScriptWithValueReturned(string script)
        {
            IJavaScriptExecutor scriptExecutor = (IJavaScriptExecutor)driver;
            return scriptExecutor.ExecuteScript(script).ToString();
        }

        public void ExecuteScript(string script)
        {
            IJavaScriptExecutor scriptExecutor = (IJavaScriptExecutor)driver;
            scriptExecutor.ExecuteScript(script);
        }
        
        public string GetCurrentWindowURL()
        {
            return driver.Url;
        }

        public void SwitchToNewWindow()
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public void SwitchToWindowByPageTitle(string pageTitle)
        {
            driver.SwitchTo().Window(pageTitle);
        }       

        public void SwitchToWindowByNumber(int num)
        {
            driver.SwitchTo().Window(driver.WindowHandles[num]);
        }

        public void SwitchToLastWindow()
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public void CloseWindow()
        {
            driver.Close();
        }

        public void ClosePopUpBlocker()
        {
            string JS_DISABLE_UNLOAD_DIALOG =
                "Object.defineProperty(BeforeUnloadEvent.prototype, 'returnValue', { get:function(){}, set:function(){} })";
            ((IJavaScriptExecutor)driver).ExecuteScript(JS_DISABLE_UNLOAD_DIALOG);
            driver.Quit();
        }

        public string GetParentWindow()
        {
            string parentwindow = driver.CurrentWindowHandle;
            return parentwindow;
        }

        public void NavigateToParentWindow(string parentWindow)
        {
            driver.SwitchTo().Window(parentWindow);
        }

        public bool IsAlertPresent()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();                
                return true;
            }
            catch (Exception e) { return false; } 
        }

        public bool SwitchToDefaultFrame()
        {
            bool flag = false;
            try
            {
                driver.SwitchTo().DefaultContent();
                flag = true;
                return flag;
            }
            catch (Exception e)
            {
                return flag;
            }
        }

        public bool SwitchToFrameByID(int IDValue)
        {
            bool flag = false;
            try
            {
                driver.SwitchTo().Frame(IDValue);
                return flag = true;
            }
            catch (Exception e)
            {
                return flag;
            }
        }

        public bool SwitchToFrameByIndex(int index)
        {
            bool flag = false;
            try
            {
                driver.SwitchTo().Frame(index);
                return flag = true;
            }
            catch (Exception e)
            {
                return flag;
            }

        }

        public bool SwitchToFrameByLocator(IWebElement locator)
        {
            bool flag = false;
            try
            {
                driver.SwitchTo().Frame(locator);
                return flag = true;
            }
            catch (Exception e)
            {
                return flag;
            }
        }

        public bool SwitchToFrameByName(string nameValue)
        {
            bool flag = false;
            try
            {
                driver.SwitchTo().Frame(nameValue);
                return flag = true;
            }
            catch (Exception e)
            {
                return flag;
            }
        }

        public void AcceptAlert()
        {
            try
            {
                IAlert simpleAlert = driver.SwitchTo().Alert();
                simpleAlert.Accept();
            }
            catch (Exception e)
            {
                throw new Exception("Alert is not handled");
            }
        }

        public void DismissAlert()
        {
            try
            {
                IAlert simpleAlert = driver.SwitchTo().Alert();
                simpleAlert.Dismiss();
            }
            catch (Exception e)
            {
                throw new Exception("Alert is not handled");
            }
        }

        public string AlertsMessage()
        {

            string text = "";
            try
            {
                IAlert simpleAlert = driver.SwitchTo().Alert();
                text = simpleAlert.Text;
            }
            catch (Exception e)
            {
                throw new Exception("Alert is not handled");
            }
            return text;
        }

        public string GetPageTitle()
        {
            string title = "";
            try
            {
                title = driver.Title;
            }
            catch (Exception e)
            {
                throw new Exception("exception happended while retrieving title of the page");
            }
            return title;
        }

        public string GetMainWindow()
        {
            string text = "";
            try
            {
                text = driver.CurrentWindowHandle;
            }
            catch (Exception e)
            {
                throw new Exception("Exception while getting window handle ");
            }
            return text;
        }

        public string SwitchToParentWindow(string mainWindow)
        {
            string text = "";
            try
            {
                driver.SwitchTo().Window(mainWindow);
            }
            catch (Exception e)
            {
                throw new Exception("Exception while getting window handle");
            }
            return text;
        }

        public void SwitchToChildWindow(string mainWindow, int window)
        {            
            try
            {
                IReadOnlyCollection<String> s1 = driver.WindowHandles;

                IList<String> list = s1.ToList();

                string[] arr = list.ToArray();

                string childWindow = arr[window];

                if (!mainWindow.Equals(childWindow))
                {
                    driver.SwitchTo().Window(childWindow);
                }
            }
            catch (Exception e)
            {
                throw new Exception("error while handling child windows");
            }
        }

        public void CloseBrowser()
        {
            driver.Close();
        }

        public void PageRefresh()
        {
            driver.Navigate().Refresh();
        }

        public void BackNavigation()
        {
            driver.Navigate().Back();
        }

        public void ForwardNavigation()
        {
            driver.Navigate().Forward();
        }
    }
}