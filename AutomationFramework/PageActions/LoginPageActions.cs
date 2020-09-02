using AutomationFramework.Extensions;
using AutomationFramework.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.PageActions
{
    class LoginPageActions
    {
        IWebDriver driver;

        public LoginPageActions(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void LoginWithValidUserNameAndPassword()
        {
            LoginPage loginPage = new LoginPage(driver);
            WebElementExtensions webElementExtensions = new WebElementExtensions(driver); 

            string userName = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];
            //loginPage.UserName.SendKeys(userName);
            //loginPage.Password.SendKeys(password);

            webElementExtensions.EnterData(loginPage.UserName, userName);
            webElementExtensions.EnterData(loginPage.Password, password);
            loginPage.LoginButton.Click();

            //.Click();
        }
    }
}
