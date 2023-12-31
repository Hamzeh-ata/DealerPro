﻿using DealerPro.fireBase;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using test;

namespace DealerPro.crawlersServices.HMG
{
    public class HMGDataCrawler
    {
        private HashSet<string> storedproducts = new HashSet<string>();

        private readonly FireBase _firebase;
        public HMGDataCrawler()
        {
        }
        public HMGDataCrawler(FireBase firebase)
        {
            _firebase = firebase;

        }

        public async Task CrawlerAsync(string url, string endPoint, string storeName)
        {
            var chromeOptions = new ChromeOptions();
            var service = ChromeDriverService.CreateDefaultService(@"D:/chromeDriver/chromedriver.exe");
            List<string> liTexts = new List<string>();
            bool flag = true;
            // matches any sequence of digits, commas, and dots
            string regexPattern = @"[\d,.]+";
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("window-size=1920,1080");
            using (var driver = new ChromeDriver(service, chromeOptions))
            {
                var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 120));
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(12000);
                Actions actions = new Actions(driver);
                try
                {
                    Thread.Sleep(5000);
                    // Find the select element by its name
                    if (driver.FindElements(By.CssSelector(".electro-wc-wppp-select")).Count > 0)
                    {
                        var selectElement = wait.Until(driver => driver.FindElement(By.CssSelector(".electro-wc-wppp-select")));
                        // Use SelectElement to wrap the select element
                        var select = new SelectElement(selectElement);
                        // Select the option with value "-1"
                        select.SelectByValue("-1");
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
                    if (driver.FindElements(By.Id("u_0_0_Nb")).Count > 0)
                    {
                        Thread.Sleep(5000);

                        IWebElement closeButton = driver.FindElement(By.Id("u_0_0_Nb")).FindElement(By.TagName("svg"));
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].scrollIntoView();", closeButton);
                        actions.MoveToElement(closeButton).Click().Build().Perform();
                    }
                }
                catch (NoSuchElementException ex)
                {
                    //handle if pop-up is not present
                    Console.WriteLine("Element not found: " + ex.Message);
                }
                while (flag)
                {
                    try
                    {
                        Thread.Sleep(5000);
                        var product = driver.FindElements(By.CssSelector(".site-main > ul.products li.product > .product-item__outer > .product-item__inner"));
                        int productsCount = product.Count();
                        for (int i = 0; i < productsCount; i++)
                        {
                            Thread.Sleep(5000);
                            Console.WriteLine("the number of all products is " + productsCount + "The number of i is " + i);
                            // Scroll the element into view using JavaScript
                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(product[i]));
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript("arguments[0].scrollIntoView();", product[i]);
                            //  open the link in a new tab using JavaScript
                            driver.ExecuteScript("window.open(arguments[0]);", product[i].FindElement(By.CssSelector("a.woocommerce-LoopProduct-link")).GetAttribute("href"));

                            // switch to the new tab
                            driver.SwitchTo().Window(driver.WindowHandles.Last());
                            // Wait for the element to become clickable
                            Thread.Sleep(10000);
                            if (driver.FindElements(By.CssSelector("div.info-404")).Count > 0)
                            {
                                driver.Close();
                                driver.SwitchTo().Window(driver.WindowHandles.First());
                            }
                            else
                            {

                                var Name = wait.Until(driver => driver.FindElement(By.CssSelector(".summary > h1.product_title")));
                                var Image = wait.Until(driver => driver.FindElement(By.CssSelector(".woocommerce-product-gallery__image > img.wp-post-image")));
                                string oldPrice = "0";
                                string currentPrice = "0";
                                if (wait.Until(driver => driver.FindElements(By.CssSelector(".entry-summary p.price > span.electro-price del > .woocommerce-Price-amount > bdi "))).Count > 0)
                                {
                                    oldPrice = wait.Until(driver => driver.FindElement(By.CssSelector(".entry-summary p.price > span.electro-price del > .woocommerce-Price-amount > bdi "))).Text;
                                    currentPrice = wait.Until(driver => driver.FindElement(By.CssSelector(".entry-summary p.price > span.electro-price ins > .woocommerce-Price-amount > bdi "))).Text;
                                }
                                else
                                {
                                    oldPrice = "0";
                                    currentPrice = wait.Until(driver => driver.FindElement(By.CssSelector(".entry-summary p.price > span.electro-price > .woocommerce-Price-amount > bdi "))).Text;
                                }
                                string currentPriceWithoutCurrency = Regex.Match(currentPrice, regexPattern).Value;
                                string oldPriceWithoutCurrency = Regex.Match(oldPrice, regexPattern).Value;
                                var brand = "Off-brand";
                                if (driver.FindElements(By.CssSelector(".entry-summary .brand img")).Count > 0)
                                {
                                    brand = wait.Until(driver => driver.FindElement(By.CssSelector(".entry-summary .brand img"))).GetAttribute("alt");
                                }
                                string productNumber = wait.Until(driver => driver.FindElement(By.CssSelector(".entry-summary > .woocommerce-product-details__short-description"))).Text;


                                string productUrl = driver.Url;
                                if (driver.FindElements(By.Id("tab-title-specification")).Count > 0)
                                {
                                    Thread.Sleep(5000);
                                    var element = driver.FindElement(By.CssSelector("li.specification_tab "));
                                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'auto', block: 'center', inline: 'center'});", element);
                                    Thread.Sleep(5000);
                                    element.Click();
                                    Thread.Sleep(5000);
                                    // wait for the table to be visible
                                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("tab-specification")));
                                    var table = driver.FindElement(By.Id("tab-specification"));
                                    // get all the th and td elements in the table and save them as a list of objects
                                    var rows = table.FindElements(By.TagName("tr"));
                                    var dataList = new List<object>();

                                    foreach (var row in rows)
                                    {
                                        var cells = row.FindElements(By.TagName("td"));
                                        var headers = row.FindElements(By.TagName("th"));
                                        liTexts.Add(headers[0].Text + " : " + cells[0].Text);
                                    }
                                }
                                else
                                {
                                    liTexts.Add("No description");
                                }
                                Thread.Sleep(5000);
                                await _firebase.InsertDataIntoFirebase(endPoint, storeName, Name.Text, Image.GetAttribute("src"), oldPriceWithoutCurrency.Trim(), currentPriceWithoutCurrency.Trim(), brand, liTexts, productUrl, productNumber);
                                storedproducts.Add(productNumber);
                                Thread.Sleep(5000);
                                // close the tab
                                driver.Close();
                                liTexts.Clear();


                                // switch back to the original tab
                                driver.SwitchTo().Window(driver.WindowHandles.First());
                                // Console.WriteLine(totalProducts + " number of products");
                                Thread.Sleep(5000);
                                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".site-main > ul.products li.product > .product-item__outer > .product-item__inner")));
                            }
                            if (i >= productsCount - 1)
                            {
                                await _firebase.CheckIfElementIsStillExistsByProductNumber(endPoint, storeName, storedproducts);
                                storedproducts.Clear();
                                Thread.Sleep(5000);
                                Console.WriteLine("Finshed");
                                flag = false;
                                driver.Quit();
                                break;
                            }
                        }
                    }
                    catch (NoSuchElementException ex)
                    {
                        Console.WriteLine("NoSuchElementExceptionn: " + ex.Message);
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
                }
            }

        }
    }
}
