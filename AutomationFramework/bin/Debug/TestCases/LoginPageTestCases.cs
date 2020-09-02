using AutomationFramework.PageActions;
using AutomationFramework.TestBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.TestCases
{
    [TestClass]
    public class LoginPageTestCases:TestCasesBase
    {
        [TestMethod]
        public void VerifyLogin()
        {
            LoginPageActions loginPageActions = new LoginPageActions(driver);
            loginPageActions.LoginWithValidUserNameAndPassword();
            
        }
    }
}
