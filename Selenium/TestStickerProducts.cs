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
    public class TestStickerProducts
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
            driver.Url = "http://localhost/litecart/";

            var Products = driver.FindElements(By.CssSelector("li.product.column.shadow.hover-light"));
            foreach (IWebElement Product in Products)
            {
                var Stickers = Product.FindElements(By.CssSelector("div.sticker"));
                if (Stickers.Count!=1)
                    Assert.Fail("Отсутствует либо не один стикер на продукте '"+ Product.FindElement(By.CssSelector("div.name")).Text+"'");
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