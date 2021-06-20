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
    public class TestPageOpening
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
            //driver = new InternetExplorerDriver();
            //driver = new FirefoxDriver();
            

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test()
        {
            driver.Url = "http://localhost/litecart/";

            //Название
            IWebElement Product = driver.FindElements(By.CssSelector("div#box-campaigns ul li.product a.link"))[0];
            string name = Product.FindElement(By.CssSelector("div.name")).Text;

            //цены
            IWebElement regular_price_c = Product.FindElement(By.CssSelector("div.price-wrapper s.regular-price"));
            string regular_price = regular_price_c.Text;
            IWebElement campaign_price_c = Product.FindElement(By.CssSelector("div.price-wrapper strong.campaign-price"));
            string campaign_price = campaign_price_c.Text;

            //цвет основной цены серая и зачеркнутость
            string[] regular_price_color = regular_price_c.GetCssValue("color").Split(',');
            if (!(string.Join("", regular_price_color[0].Where(c => char.IsDigit(c))) == string.Join("", regular_price_color[1].Where(c => char.IsDigit(c))) &&
                string.Join("", regular_price_color[2].Where(c => char.IsDigit(c))) == string.Join("", regular_price_color[1].Where(c => char.IsDigit(c)))))
                Assert.Fail("Основная цена не серая");

            if (!regular_price_c.GetCssValue("text-decoration").Contains("line-through"))
                Assert.Fail("Основная цена не зачеркнута");

            //акционная жирная и красная
            string[] campaign_price_color = campaign_price_c.GetCssValue("color").Split(',');
            if (!(string.Join("", campaign_price_color[1].Where(c => char.IsDigit(c))) == string.Join("", campaign_price_color[2].Where(c => char.IsDigit(c))) &&
                string.Join("", campaign_price_color[1].Where(c => char.IsDigit(c))) == "0"))
                Assert.Fail("Акционная цена не красная");

            string campaign_price_bold = campaign_price_c.GetCssValue("font-weight");
            if (int.TryParse(campaign_price_bold, out int Digi_bold))
            {
                if (Digi_bold < 700)
                    Assert.Fail("Акционная цена не выделана жирным");
            }
            else if (campaign_price_bold != "bold")
                Assert.Fail("Акционная цена не выделана жирным");

            //акционная цена крупнее, чем обычная
            if (float.Parse(string.Join("", campaign_price_c.GetCssValue("font-size").Where(c => !char.IsLetter(c))).Replace('.', ',')) <= float.Parse(string.Join("", regular_price_c.GetCssValue("font-size").Where(c => !char.IsLetter(c))).Replace('.', ',')))
                Assert.Fail("Акционная цена имеет меньший шрифт чем обычная");

            Product.Click(); //страница продукта
            //Название
            if (driver.FindElement(By.CssSelector("h1.title")).Text != name)
                Assert.Fail("Не совпадает наимаенование товара в списке и на его странице");

            //цены
            IWebElement regular_price_c_ = driver.FindElement(By.CssSelector("div.price-wrapper s.regular-price"));
            IWebElement campaign_price_c_ = driver.FindElement(By.CssSelector("div.price-wrapper strong.campaign-price"));
            
            if (regular_price_c_.Text != regular_price)
                Assert.Fail("Не совпадает основная цена");

            if (campaign_price_c_.Text != campaign_price)
                Assert.Fail("Не совпадает акционная цена");

            //цвет основной цены серая и зачеркнутость
            string[] regular_price_color_ = regular_price_c_.GetCssValue("color").Split(',');
            if (!(string.Join("", regular_price_color_[0].Where(c => char.IsDigit(c))) == string.Join("", regular_price_color_[1].Where(c => char.IsDigit(c))) &&
                string.Join("", regular_price_color_[2].Where(c => char.IsDigit(c))) == string.Join("", regular_price_color_[1].Where(c => char.IsDigit(c)))))
                Assert.Fail("Основная цена не серая");

            if (!regular_price_c_.GetCssValue("text-decoration").Contains("line-through"))
                Assert.Fail("Основная цена не зачеркнута");

            //акционная жирная и красная
            string[] campaign_price_color_ = campaign_price_c_.GetCssValue("color").Split(',');
            if (!(string.Join("", campaign_price_color_[1].Where(c => char.IsDigit(c))) == string.Join("", campaign_price_color_[2].Where(c => char.IsDigit(c))) &&
                string.Join("", campaign_price_color_[1].Where(c => char.IsDigit(c))) == "0"))
                Assert.Fail("Акционная цена не красная");

            string campaign_price_bold_ = campaign_price_c_.GetCssValue("font-weight");
            if (int.TryParse(campaign_price_bold_, out int Digi_bold_))
            {
                if (Digi_bold_ < 700)
                    Assert.Fail("Акционная цена не выделана жирным");
            }
            else if (campaign_price_bold_ != "bold")
                Assert.Fail("Акционная цена не выделана жирным");

            //акционная цена крупнее, чем обычная
            if (float.Parse(string.Join("", campaign_price_c_.GetCssValue("font-size").Where(c => !char.IsLetter(c))).Replace('.', ',')) <= float.Parse(string.Join("", regular_price_c_.GetCssValue("font-size").Where(c => !char.IsLetter(c))).Replace('.', ',')))
                Assert.Fail("Акционная цена имеет меньший шрифт чем обычная");
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}