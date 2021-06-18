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
    public class TestSort
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test1()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            string prev_country_name=null;

            var countries = driver.FindElements(By.CssSelector("table.dataTable tr.row"));
            for (int i = 0; i < countries.Count; i++)
            {
                IWebElement country = driver.FindElements(By.CssSelector("table.dataTable tr.row"))[i];
                IWebElement colName = country.FindElement(By.CssSelector("td a"));
                string country_name = colName.Text;
                if (!string.IsNullOrEmpty(prev_country_name))
                {
                    if (country_name.CompareTo(prev_country_name) < 0)
                        Assert.Fail("Неверная сортировка в паре стран '" + prev_country_name + "' и '" + country_name + "'");
                }
                prev_country_name = country_name;

                if (Convert.ToInt16(country.FindElements(By.CssSelector("td"))[5].Text) > 0)
                {
                    country.FindElement(By.CssSelector("td a")).Click();
                    //проверка зон
                    string prev_zone_name = null;
                    var rows = driver.FindElements(By.CssSelector("table.dataTable tr"));
                    foreach (IWebElement row in rows)
                    {
                        if (row.GetAttribute("class") == "")
                        {
                            IWebElement zone = row.FindElement(By.CssSelector("td input[name*=name]"));
                            if (zone.GetAttribute("type") == "hidden")
                            {
                                string zone_name = row.FindElements(By.CssSelector("td"))[2].Text;
                                if (!string.IsNullOrEmpty(prev_zone_name))
                                {
                                    if (zone_name.CompareTo(prev_zone_name) < 0)
                                        Assert.Fail("Неверная сортировка в паре зон '" + prev_zone_name + "' и '" + zone_name + "'");
                                }
                                prev_zone_name = zone_name;
                            }
                        }
                    }
                    //возвращаемся
                    driver.Navigate().Back();
                }
            }

        }

        [Test]
        public void Test2()
        {
            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            var countries = driver.FindElements(By.CssSelector("table.dataTable tr.row"));
            for (int i = 0; i < countries.Count; i++)
            {
                IWebElement country = driver.FindElements(By.CssSelector("table.dataTable tr.row"))[i];
                IWebElement colName = country.FindElements(By.CssSelector("td a"))[0];
                colName.Click();

                //проверка зон
                string prev_zone_name = null;
                var zones = driver.FindElements(By.CssSelector("table#table-zones tr td select[name*=zone_code] option[selected=selected]"));
                foreach (IWebElement zone in zones)
                {
                    string zone_name = zone.Text;

                    if (!string.IsNullOrEmpty(prev_zone_name))
                    {
                        if (zone_name.CompareTo(prev_zone_name) < 0)
                            Assert.Fail("Неверная сортировка в паре зон '" + prev_zone_name + "' и '" + zone_name + "'");
                    }
                    prev_zone_name = zone_name;

                }
                //возвращаемся
                driver.Navigate().Back();

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