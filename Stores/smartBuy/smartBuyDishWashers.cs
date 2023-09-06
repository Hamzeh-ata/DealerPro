using Microsoft.AspNetCore.SignalR;
using test.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using DealerPro.fireBase;
using DealerPro.crawlersServices.smartBuy;

namespace test.smartBuy
{
    public class smartBuyDishWashers : Hub, IsmartBuyDishWashers
    {
        private HashSet<string> storedproducts = new HashSet<string>();
        string endPoint = "Products/Dishwashers/";
        string storeName = "SmartBuy";
        string url = "https://smartbuy-me.com/smartbuystore/ar/%D8%A7%D9%84%D8%A7%D8%AC%D9%87%D8%B2%D8%A9-%D8%A7%D9%84%D9%85%D9%86%D8%B2%D9%84%D9%8A%D8%A9/%D8%A7%D9%84%D8%A7%D8%AC%D9%87%D8%B2%D8%A9-%D8%A7%D9%84%D9%83%D8%A8%D9%8A%D8%B1%D8%A9/%D8%AC%D9%84%D8%A7%D9%8A%D8%A7%D8%AA/c/30104";
        private FireBase firebase;
        private smartBuyDataCrawler crawler;
        public smartBuyDishWashers(FireBase firebase, smartBuyDataCrawler crawler)
        {
            this.firebase = firebase;
            this.crawler = crawler;
        }
        public async Task getDishWasher()
        {
            await crawler.CrawlerAsync(url, endPoint, storeName);
        }
    }
}
