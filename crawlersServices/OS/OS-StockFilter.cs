using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace DealerPro.crawlersServices.OS
{
    public static class OS_StockFilter
    {
        public static void SetStockFilter(WebDriverWait wait, Actions actions)
        {
            try { 
            Thread.Sleep(10000);
            IWebElement stockStatusForm = wait.Until(driver => driver.FindElement(By.CssSelector(".bf-form ")));
            IWebElement stockStatusHeader = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".bf-attr-block >.bf-attr-header.bf-clickable")));
            actions.MoveToElement(stockStatusHeader).Click().Build().Perform();
            Thread.Sleep(10000);
            IWebElement inStockCheckbox = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("#bf-attr-s0_7_60")));
            if (!inStockCheckbox.Selected)
            {
                inStockCheckbox.Click();
                Thread.Sleep(5000);
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
        }

    }
}
