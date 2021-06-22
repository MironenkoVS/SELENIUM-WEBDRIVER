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
    public class TestNewUser
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        private string randomstring(int Length)
        {
            string randomstring = "";
            const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random rnd = new Random();
            for (int i = 0; i < Length; i++)
            {
                randomstring+=AllowedChars[rnd.Next(AllowedChars.Length)];
            }

            return randomstring;
        }

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
        }

        [Test]
        public void Test()
        {
            driver.Url = "http://localhost/litecart/en/create_account";

            driver.FindElement(By.CssSelector("input[name=firstname]")).SendKeys("firstname");
            driver.FindElement(By.CssSelector("input[name=lastname]")).SendKeys("lastname");
            driver.FindElement(By.CssSelector("input[name=address1]")).SendKeys("address1");
            driver.FindElement(By.CssSelector("input[name=postcode]")).SendKeys("12345");
            driver.FindElement(By.CssSelector("input[name=city]")).SendKeys("city");
            driver.FindElement(By.CssSelector("span[role=combobox]")).Click();
            driver.FindElement(By.CssSelector("li.select2-results__option[id*=-US]")).Click();
            string email = randomstring(5) + "@email.com";
            driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(email);
            driver.FindElement(By.CssSelector("input[name=phone]")).SendKeys("123456");
            string password = "123456";
            driver.FindElement(By.CssSelector("input[name=password]")).SendKeys(password);
            driver.FindElement(By.CssSelector("input[name=confirmed_password]")).SendKeys(password);

            driver.FindElement(By.CssSelector("button[name=create_account]")).Click();
            
            driver.FindElements(By.CssSelector(".list-vertical li a")).Where(a => a.Text == "Logout").First().Click();

            driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(email);
            driver.FindElement(By.CssSelector("input[name=password]")).SendKeys(password);
            driver.FindElement(By.CssSelector("button[name=login]")).Click();

            driver.FindElements(By.CssSelector(".list-vertical li a")).Where(a => a.Text == "Logout").First().Click();

        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}