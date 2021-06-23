using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Drawing;
using System.Linq;
using System.IO;

namespace Tests
{
    [TestFixture]
    public class TestNewProduct
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
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            var apps = driver.FindElements(By.CssSelector("li#app-"));
            apps.Where(li => li.FindElement(By.CssSelector("span.name")).Text == "Catalog").First().Click();

            driver.FindElements(By.CssSelector("a.button")).Where(a => a.Text.Contains("Add New Product")).First().Click();

            //General
            driver.FindElement(By.CssSelector("input[name=status][value='1']")).Click();
            string name = "Duck in suit";
            driver.FindElement(By.CssSelector("input[name='name[en]']")).SendKeys(name);
            Random rnd = new Random();
            string code = rnd.Next(10000, 99999).ToString();
            driver.FindElement(By.CssSelector("input[name='code']")).SendKeys(code);
            driver.FindElement(By.CssSelector("input[name='categories[]'][value='1']")).Click();
            var quantity = driver.FindElement(By.CssSelector("input[name='quantity']"));
            quantity.Clear();
            quantity.SendKeys("100");
            driver.FindElement(By.CssSelector("input[name='new_images[]'")).SendKeys(Path.GetFullPath("../../../duck.png"));


            //Information
            driver.FindElements(By.CssSelector("ul.index a")).Where(a => a.Text=="Information").First().Click();
            var selectElement = new SelectElement(driver.FindElement(By.CssSelector("select[name=manufacturer_id]")));
            selectElement.SelectByValue("1");
            driver.FindElement(By.CssSelector("input[name='short_description[en]']")).SendKeys("Duck in suit");

            driver.FindElement(By.CssSelector("div.trumbowyg-editor")).SendKeys(
                @"Long text to fill in the description field has no requirements, so I write what comes to my mind.
This proposal is in order to increase the volume, although it is not clear whether this is necessary."
                );

            //и Prices.Скидки(Campains) на вкладке Prices можно не добавлять.
            driver.FindElements(By.CssSelector("ul.index a")).Where(a => a.Text == "Prices").First().Click();
            var price = driver.FindElement(By.CssSelector("input[name='purchase_price']"));
            price.Clear();
            price.SendKeys("100");
            selectElement = new SelectElement(driver.FindElement(By.CssSelector("select[name=purchase_price_currency_code]")));
            selectElement.SelectByValue("USD");
            driver.FindElement(By.CssSelector("input[name='prices[USD]']")).SendKeys("17");

            driver.FindElement(By.CssSelector("button[name=save]")).Click();

            //Проверяем в каталоге
            var prodlist = driver.FindElements(By.CssSelector("table.dataTable tr.row a")).Where(a => a.Text == name);
            if (prodlist.Count()<1)
                Assert.Fail("Ошибка создания продукта");

        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}