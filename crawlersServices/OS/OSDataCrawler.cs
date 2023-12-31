﻿using DealerPro.fireBase;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using test;

namespace DealerPro.crawlersServices.OS
{
    public class OSDataCrawler
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        private readonly FireBase _firebase;

        public OSDataCrawler()
        {
        }
        public OSDataCrawler(FireBase firebase)
        {
            _firebase = firebase;
        }
        public async Task CrawlerAsync(string url, string endPoint, string storeName)
        {
            var chromeOptions = new ChromeOptions();
            var service = ChromeDriverService.CreateDefaultService(@"C:/Users/tsmra/Desktop/chromedriver.exe");
            List<string> liTexts = new List<string>();
               chromeOptions.AddArgument("--headless");
              chromeOptions.AddArgument("--disable-gpu");
              chromeOptions.AddArgument("--no-sandbox");
              chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("window-size=1920,1080");
            using (var driver = new ChromeDriver(service, chromeOptions))
            {
               // fireBaseServices _fireBaseServices = new fireBaseServices();
                var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 120));
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(12000);
                Actions actions = new Actions(driver);
                OS_StockFilter.SetStockFilter(wait, actions);
                while (true)
                {
                    try
                    {
                        Thread.Sleep(5000);
                        var product = driver.FindElements(By.CssSelector(".product-thumb"));
                        for (int i = 0; i < product.Count(); i++)
                        {
                            Thread.Sleep(5000);
                            string oldPriceBeforeClick = "0";
                            if (wait.Until(driver => product[i].FindElements(By.CssSelector("p.price > span.price-old"))).Count > 0)
                            {
                                var oldPriceParentB = wait.Until(driver => product[i].FindElement(By.CssSelector("p.price > span.price-old")));
                                var priceIntB = wait.Until(driver => oldPriceParentB.FindElement(By.CssSelector(".tb_integer")));
                                var priceDecimalOldB = wait.Until(driver => oldPriceParentB.FindElement(By.CssSelector(".tb_decimal")));
                                oldPriceBeforeClick = priceIntB.Text + "." + priceDecimalOldB.Text;
                            }
                            else
                            {
                                oldPriceBeforeClick = "0";
                            }
                            var priceParentB = wait.Until(driver => product[i].FindElement(By.CssSelector(".price")));
                            var priceSpanB = wait.Until(driver => priceParentB.FindElements(By.CssSelector("span")).Last(e => e.GetAttribute("class") == "tb_integer"));
                            var priceDecimalB = wait.Until(driver => priceParentB.FindElements(By.CssSelector("span.tb_decimal")).Last());
                            string priceBeforeClick = priceSpanB.Text + "." + priceDecimalB.Text;
                            var NameBeforeClick = product[i].FindElement(By.CssSelector("div.caption > h4"));
                            storedproducts.Add(NameBeforeClick.Text);

                            if (await _firebase.CheckIfAlreadyExists(endPoint, storeName, NameBeforeClick.Text, oldPriceBeforeClick, priceBeforeClick))
                            {
                                Console.WriteLine("The element and the price are already the same and exists");
                                continue;
                            }
                            else
                            {
                                // Scroll the element into view using JavaScript
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("arguments[0].scrollIntoView();", product[i]);
                                // Wait for the element to become clickable
                                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(product[i]));
                                //click on Computer
                                actions.MoveToElement(product[i].FindElement(By.CssSelector("div.caption > h4 > a"))).Click().Build().Perform();
                                Thread.Sleep(10000);
                                //get chair name
                                var productName = wait.Until(driver => driver.FindElement(By.CssSelector("#PageTitleSystem_OCL5xk7q > h1")));
                                //get mchairb img
                                var productImage = wait.Until(driver => driver.FindElement(By.CssSelector("div.tb_slides > span > img")));
                                //get chair price
                                string oldPrice = "0";
                                if (wait.Until(driver => driver.FindElements(By.CssSelector(".tb_system_product_price > .price > span.price-old"))).Count > 0)
                                {
                                    var oldPriceParent = wait.Until(driver => driver.FindElement(By.CssSelector(".tb_system_product_price > .price > span.price-old")));
                                    var priceInt = wait.Until(driver => oldPriceParent.FindElement(By.CssSelector(".tb_integer")));
                                    var priceDecimalOld = wait.Until(driver => oldPriceParent.FindElement(By.CssSelector(".tb_decimal")));
                                    oldPrice = priceInt.Text + "." + priceDecimalOld.Text;
                                }
                                else
                                {
                                    oldPrice = "0";
                                }
                                var priceParent = wait.Until(driver => driver.FindElement(By.CssSelector(".price")));
                                var priceSpan = wait.Until(driver => priceParent.FindElements(By.CssSelector("span")).Last(e => e.GetAttribute("class") == "tb_integer"));
                                var priceDecimal = wait.Until(driver => priceParent.FindElements(By.CssSelector("span.tb_decimal")).Last());
                                string newPrice = priceSpan.Text + "." + priceDecimal.Text;
                                //get desc
                                // Find the table element
                                Thread.Sleep(5000);
                                IWebElement aParent = driver.FindElement(By.CssSelector("#Group_AYu75Txx > .tb_tabs > ul.nav-tabs li"));

                                if (driver.FindElements(By.CssSelector("ul.nav-tabs li > a[href='Ua8dG_tab']")).Count > 0)
                                {
                                    Console.WriteLine("ee");
                                    IWebElement aElement = driver.FindElement(By.CssSelector(" ul.nav-tabs li > a[href='Ua8dG_tab']"));
                                    IWebElement parent = aElement.FindElement(By.XPath(".."));
                                    string classValue = parent.GetAttribute("class");
                                    if (!classValue.Contains("active") && !driver.FindElement(By.CssSelector("table.table")).Displayed)
                                    {
                                        Thread.Sleep(5000);
                                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(aElement));
                                        actions.MoveToElement(aElement).Click().Build().Perform();
                                        Thread.Sleep(5000);
                                        actions.MoveToElement(driver.FindElement(By.CssSelector("table.table"))).Click().Build().Perform();
                                        IWebElement table = wait.Until(driver => driver.FindElement(By.CssSelector("table.table")));
                                        ReadOnlyCollection<IWebElement> rows = wait.Until(driver => table.FindElements(By.CssSelector("tbody > tr ")));
                                        // Find all the td elements within the table
                                        foreach (IWebElement td in rows)
                                        {
                                            ReadOnlyCollection<IWebElement> cells = td.FindElements(By.TagName("td"));
                                            if (cells.Count > 0)
                                            {
                                                string heading = cells[0].Text;
                                                string value = cells[1].Text;

                                                var rowObject = new
                                                {
                                                    Label = heading,
                                                    Value = value
                                                };
                                                liTexts.Add(heading + " : " + value);
                                            }
                                        }
                                    }
                                }
                                else if (driver.FindElements(By.CssSelector("table.table")).Count > 0 && driver.FindElement(By.CssSelector("table.table")).Displayed)
                                {
                                    actions.MoveToElement(driver.FindElement(By.CssSelector("table.table"))).Click().Build().Perform();
                                    IWebElement table = wait.Until(driver => driver.FindElement(By.CssSelector("table.table")));
                                    ReadOnlyCollection<IWebElement> rows = wait.Until(driver => table.FindElements(By.CssSelector("tbody > tr ")));
                                    // Find all the td elements within the table
                                    foreach (IWebElement td in rows)
                                    {
                                        ReadOnlyCollection<IWebElement> cells = td.FindElements(By.TagName("td"));
                                        if (cells.Count > 0)
                                        {
                                            string heading = cells[0].Text;
                                            string value = cells[1].Text;

                                            var rowObject = new
                                            {
                                                Label = heading,
                                                Value = value
                                            };
                                            liTexts.Add(heading + " : " + value);
                                        }
                                    }
                                }
                                else if (driver.FindElements(By.CssSelector("#ProductDescriptionSystem_Bx7ALQdc > .tb_product_description ")).Count > 0 && driver.FindElements(By.CssSelector(" ul.nav-tabs li > a[href='Ua8dG_tab']")).Count <= 0)
                                {
                                    actions.MoveToElement(driver.FindElement(By.CssSelector("#ProductDescriptionSystem_Bx7ALQdc"))).Perform();

                                    // Find the div with class "specs__description"
                                    // Find the div element by its class name
                                    IWebElement divElement = driver.FindElement(By.CssSelector("#ProductDescriptionSystem_Bx7ALQdc > .tb_product_description"));

                                    // Get all the child elements of the div element
                                    liTexts.Add(divElement.Text);

                                }
                                else
                                {
                                    liTexts.Add("No description");
                                }
                                string productUrl = driver.Url;
                                //get Computer stock
                                var productStock = wait.Until(driver => driver.FindElements(By.CssSelector("span"))
                                                                  .Where(e => e.GetAttribute("class").Contains("tb_stock_status"))
                                                                  .FirstOrDefault());

                                //GET BRAND 
                                IWebElement productInfoSystem = driver.FindElement(By.Id("ProductInfoSystem_Ho7r8pnm"));

                                // Find all the dd elements within the div
                                IList<IWebElement> ddElements = productInfoSystem.FindElements(By.TagName("dd"));

                                // Get the text of the last dd element
                                string brand = ddElements[ddElements.Count - 1].Text;

                                //insert Keyboard into firebase
                                await _firebase.InsertDataIntoFirebase(endPoint, storeName, productName.Text, productImage.GetAttribute("src"), oldPrice, newPrice, productStock.Text, brand, liTexts, productUrl);
                                driver.Navigate().Back();

                                liTexts.Clear();
                                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".product-thumb")));
                                product = driver.FindElements(By.CssSelector(".product-thumb"));

                            }
                        }
                    }
                    catch (NoSuchElementException ex)
                    {
                        // Handle the exception - e.g. log the error or show an error message
                        Console.WriteLine("Element not found: " + ex.Message);
                    }
                    catch (WebDriverException ex)
                    {
                        // Handle the exception - e.g. log the error or show an error message
                        Console.WriteLine("WebDriver exception: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions - e.g. log the error or show an error message
                        Console.WriteLine("Exception: " + ex.Message);
                    }

                    try
                    {
                        var nextButton = driver.FindElement(By.CssSelector("ul.links > li.next > a[rel='next']"));
                        if (nextButton != null)
                        {
                            actions.MoveToElement(nextButton).Click().Build().Perform();
                            Thread.Sleep(10000);
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        await _firebase.CheckIfElementIsStillExists(endPoint, storeName, storedproducts);
                        storedproducts.Clear();
                        driver.Quit();
                        Console.WriteLine("Finshed");
                        break;
                    }
                }
            }
        }
    }
}
