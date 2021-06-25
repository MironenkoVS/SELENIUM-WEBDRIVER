using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Drawing;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class TestShopCart
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
        }

        [Test]
        public void Test()
        {
            driver.Url = "http://localhost/litecart/";             

            for (int i = 0; i < 3; i++)
            {
                driver.FindElement(By.CssSelector("li.product")).Click();
                if (driver.FindElements(By.CssSelector("select[name='options[Size]']")).Count() > 0)
                {
                    SelectElement selectElement = new SelectElement(driver.FindElement(By.CssSelector("select[name='options[Size]']")));
                    selectElement.SelectByIndex(1);
                }
                driver.FindElement(By.CssSelector("button[name=add_cart_product]")).Click();
                wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("div#cart span.quantity"), (i + 1).ToString()));
                driver.Navigate().Back();
            }

            driver.FindElement(By.CssSelector("div#cart a.link")).Click();

            while (driver.FindElements(By.CssSelector("div#box-checkout-summary table tr td")).Count>0)
            {   
                driver.FindElement(By.CssSelector("button[name = remove_cart_item]")).Click();
                wait.Until(ExpectedConditions.StalenessOf(driver.FindElement(By.CssSelector("div#box-checkout-summary table tr td"))));
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