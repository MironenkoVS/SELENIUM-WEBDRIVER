using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
    [TestFixture]
    public class TestMenuList
    {
        private IWebDriver driver, ieDriver, firefoxDriver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test()
        {
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            var apps = driver.FindElements(By.CssSelector("li#app-"));
            for (int i=0; i<apps.Count; i++)
            {
                driver.FindElements(By.CssSelector("li#app-"))[i].Click();
                Assert.IsNotEmpty(driver.FindElement(By.CssSelector("h1")).Text, "Открыт пункт меню приложений " + i.ToString() + " заголовок страницы не задан");
                
                var docs = driver.FindElements(By.CssSelector("li#app- ul.docs li"));
                if (docs.Count > 0)
                {
                    for (int j = 0; j < docs.Count; j++)
                    {
                        driver.FindElements(By.CssSelector("li#app- ul.docs li"))[j].Click();
                        Assert.IsNotEmpty(driver.FindElement(By.CssSelector("h1")).Text, "Открыт пункт меню приложений " + i.ToString() + ", документ " + j.ToString() + " заголовок страницы не задан");
                    }
                }

            }
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}