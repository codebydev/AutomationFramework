using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace AutomationFramework.Extensions
{
    public class WebElementExtensions
    {
        public static ScreenshotImageFormat format = ScreenshotImageFormat.Jpeg;

        public static string ScreenShotPath = @"C:\ScreenShots" + DateTime.Now.ToString("mm-dd-yyyy", CultureInfo.InvariantCulture) + "." + ScreenshotImageFormat.Jpeg;

        IWebDriver driver;

        public WebElementExtensions(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement WaitForElement(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));

            IWebElement ele = wait.Until(driver => element);

            return ele;
        }

        public bool IsElementDisplayed(IWebElement element)
        {
            try
            {
                if (element.Displayed)
                    return true;
            }
            catch (Exception e)
            {

                throw new Exception("element is not displayed");
            }
            return false;
        }

        public bool IsElementPresent(IWebElement element)
        {
            bool isDisplayed = false;
            try
            {
                isDisplayed = element.Displayed;
            }
            catch (NoSuchElementException e)
            {
                e.Message.ToString();
                e.GetBaseException();

            }
            return isDisplayed;
        }

        public bool ElementNotPresent(IWebElement element)
        {
            return !IsElementPresent(element);

        }

        public IWebElement GetElement(IWebElement element)
        {
            if (IsElementPresent(element))
            {
                return element;
            }
            else
            {
                throw new Exception(message: $"{element.ToString()} not visible to be interacted with");
            }
        }

        public void DynamicWait(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementSelectionStateToBe(element, true));
        }

        public void WaitForAllElementsToDisplay(IList<IWebElement> elements)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            wait.Until(WaitForAllElementsVisibility(elements));
        }

        public void WaitForElementToDisappear(IWebElement element)
        {
            IWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromSeconds(90);
            wait.PollingInterval = TimeSpan.FromMilliseconds(300);
            wait.Until(WaitForElementToBeInvisible(element));
        }

        public bool WaitForElementToBeDisplayed(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.Timeout = TimeSpan.FromSeconds(120);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            return wait.Until(WaitForElementToBeSeen(element));
        }

        public IWebElement WaitForElementToBeClickable(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        public IWebElement WaitForLinkTextElementToBeClickable(string linkText)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(driver.FindElement(By.PartialLinkText(linkText))));
        }

        public bool WaitForElementToBeVisible(IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));
            return wait.Until(WaitForElementVisibility(element));
        }

        public void HardWait(object seconds)
        {
            System.Threading.Thread.Sleep(Convert.ToInt32(seconds) * 1000);
        }

        private Func<IWebElement, bool> WaitForElementToBeSeen(IWebElement element)
        {
            return ((x) =>
            {
                return element.Displayed;

            });
        }

        private Func<IWebElement, bool> WaitForElementToBeInvisible(IWebElement element)
        {
            return ((x) =>
            {
                try
                {
                    return !element.Displayed;
                }
                catch (Exception)
                {
                    return true;
                }
            });
        }

        private Func<IWebDriver, bool> WaitForElementVisibility(IWebElement element)
        {
            return ((x) =>
            {
                return element.Enabled;

            });
        }

        private Func<IWebDriver, IList<IWebElement>> WaitForAllElementsVisibility(IList<IWebElement> element)
        {
            return ((x) =>
            {
                return element.ToList();

            });
        }

        public IWebElement FluentWaitForElement(IWebElement element)
        {
            DefaultWait<IWebElement> def = new DefaultWait<IWebElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(30000),
                PollingInterval = TimeSpan.FromSeconds(20)
            };

            IWebElement ele = def.Until(driver => element);

            return ele;
        }

        public void ScrollToElement(IWebElement element)
        {
            try
            {
                Actions actions = new Actions(driver);
                actions.MoveToElement(element);
                actions.Build().Perform();
            }
            catch (Exception e)
            {
                throw new Exception("Unbale to locate the element");
            }
        }

        public void ClickOnWebElement(IWebElement element)
        {
            try
            {

                //IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                //js.ExecuteScript("arguments[0].style.border='3px solid red'", element);

                //js.ExecuteScript("arguments[0].click();", element);
                element.Click();
                
            }
            catch (Exception)
            {
                //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(ScreenShotPath, format);
            }
        }

        public void ScrollDownPage()
        {
            try
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                js.ExecuteScript("window.scrollBy(0,900)"); 
            }
            catch (Exception)
            {
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(ScreenShotPath, format);
            }
        }

        public void ScrollUpPage()
        {
            try
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                js.ExecuteScript("window.scrollBy(0,-700)");
            }
            catch (Exception)
            {
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(ScreenShotPath, format);
            }
        }
        
        public void EnterData(IWebElement element, string data)
        {
            try
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;

                js.ExecuteScript("arguments[0].style.border='2px solid yellow'", element);

                FluentWaitForElement(element).Clear();

                element.SendKeys(data);
            }
            catch (Exception e)
            {
                throw new Exception("element is not displayed");
            }
        }

        public void SelectOptionByText(IWebElement element, string text)
        {
            try
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                js.ExecuteScript("argument[0].style.border='2px solid yellow'", element);
                FluentWaitForElement(element);
                SelectElement oSelect = new SelectElement(element);
                oSelect.SelectByText(text);
            }
            catch (Exception e)
            {
                throw new Exception("element is not Selected");
            }
        }

        public void SelectOptionByIndex(IWebElement element, int value)
        {
            try
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                js.ExecuteScript("argument[0].style.border='2px solid yellow'", element);
                FluentWaitForElement(element);
                SelectElement oSelect = new SelectElement(element);
                oSelect.SelectByIndex(value);
            }
            catch (Exception e)
            {
                throw new Exception("element is not Selected");
            }
        }

        public void SelectOptionByOptions(IWebElement element, string value)
        {
            try
            {
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                js.ExecuteScript("argument[0].style.border='2px solid yellow'", element);
                FluentWaitForElement(element);
                SelectElement oSelect = new SelectElement(element);
                oSelect.SelectByValue(value);
            }
            catch (Exception e)
            {
                throw new Exception("element is not Selected");
            }
        }

        public string GetText(IWebElement element)
        {
            string text = "";
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("argument[0].style.border='2px solid yellow'", element);
            try
            {
                if (IsElementDisplayed(element))
                {
                    text = element.Text;
                }
            }
            catch (Exception e)
            {
                throw new Exception("element is not present");
            }
            return text;
        }
        
        public string GetValue(IWebElement element, string value)
        {
            string text = "";
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("argument[0].style.border='2px solid yellow'", element);
            try
            {
                if (IsElementDisplayed(element))
                {
                    text = element.GetAttribute(value);
                }
            }
            catch (Exception e)
            {
                throw new Exception("element is not present");
            }
            return text;
        }

        public bool IsElementSelected(IWebElement element)
        {
            try
            {
                if (element.Selected)
                    return true;
            }
            catch (Exception e)
            {

                throw new Exception("element is not selected");
            }
            return false;
        }

        public bool IsElementEnabled(IWebElement element)
        {
            try
            {
                if (element.Enabled)
                    return true;
            }
            catch (Exception e)
            {

                throw new Exception("element is not enabled");
            }
            return false;
        }

        public bool IsElementDisabled(IWebElement element)
        {
            bool? value = true;
            try
            {
                if (element.Enabled)
                    value = false;
            }
            catch (Exception e)
            {
                value = true;
            }
            return value.Value;

        }

        public string GetTooltip(IWebElement element)
        {
            string Tooltip = "";
            try
            {
                Tooltip = element.GetAttribute("title");
            }
            catch (Exception e)
            {
                throw new Exception("could not get ToolTip");
            }
            return Tooltip;
        }

        public void HoverOnWebElement(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction action = builder.MoveToElement(element).Build();
                action.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }
        }

        public void RightClickOnElement(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction seriesofActions = builder.MoveToElement(element).SendKeys(Keys.ArrowRight).Build();
                seriesofActions.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }
        }

        public void LeftClickOnElement(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction seriesofActions = builder.MoveToElement(element).SendKeys(Keys.ArrowLeft).Build();
                seriesofActions.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }

        }

        public void ScrollDownPage(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction seriesofActions = builder.MoveToElement(element).SendKeys(Keys.PageDown).Build();
                seriesofActions.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }

        }

        public void ScrollToElement(int x, int y)
        {

            IJavaScriptExecutor javScriptExecutor = (IJavaScriptExecutor)driver;

            javScriptExecutor.ExecuteScript("window.scrollBy(" + x + ", " + y + ");");

        }
        
        public void ScrollUpToElement(IWebElement element)
        {
            try
            {
                Point point = element.Location;

                int x_coordinate = point.X;

                int y_coordinate = point.Y;


                ScrollToElement(x_coordinate, y_coordinate);

            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }

        }

        public void ScrollDown()
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollBy(0,5000)");

            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }

        }

        public void ScrollUpPage(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction seriesofActions = builder.MoveToElement(element).SendKeys(Keys.PageUp).Build();
                seriesofActions.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }
        }
        
        public void ScrollUp(IWebElement element)
        {
            try
            {
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", element);

            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }
        }

        public void ClickOnShiftKey(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction seriesofActions = builder.MoveToElement(element).SendKeys(Keys.Shift).Build();
                seriesofActions.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }

        }

        public void ClearTextField(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction seriesofActions = builder.MoveToElement(element).SendKeys(Keys.Clear).Build();
                seriesofActions.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }
        }

        public void DoubleClickOnElement(IWebElement element)
        {
            try
            {
                Actions builder = new Actions(driver);
                IAction seriesofActions = builder.MoveToElement(element).DoubleClick().Build();
                seriesofActions.Perform();
            }
            catch (Exception e)
            {
                throw new Exception("unbale to hover onto element");
            }
        }

        public string GetBackGroundColor(IWebElement element)
        {
            string color = "";
            try
            {
                color = element.GetAttribute("background-color");
            }
            catch (Exception e)
            {
                throw new Exception("unable to identify element");
            }
            return color;
        }

        public void ClickOnSubmitButton(IWebElement element)
        {
            try
            {
                FluentWaitForElement(element).Submit();
            }
            catch (Exception e)
            {
                throw new Exception("unable to perform submit action");
            }
        }

        public void ShiftFocus(IWebElement element)
        {
            try
            {
                FluentWaitForElement(element).SendKeys(Keys.Tab);
            }
            catch (Exception e)
            {
                throw new Exception("can't shift focus");
            }
        }

        public string GetTextFromDDL(IWebElement element)
        {
            return new SelectElement(element).AllSelectedOptions.SingleOrDefault().Text;
        }

        public void ScrollFocusToLocator(IWebElement element)
        {
            string js = string.Format("window.scroll(0, {0});", element.Location.Y);
            ((IJavaScriptExecutor)driver).ExecuteScript(js);
            element.Click();
        }

        public void WaitAndEnterDataInToTextField(IWebElement element, string text)
        {
            try
            {
                if (element.Displayed && element.Enabled)
                {
                    FluentWaitForElement(element).SendKeys(text);
                }
            }
            catch (Exception e)
            {
                throw new Exception("element is not present");
            }
        }

        public int GetNumberOfRowsInTable(IWebElement tableElement)
        {            
            return tableElement.FindElements(By.TagName("tr")).Count;
        }
    }
}

